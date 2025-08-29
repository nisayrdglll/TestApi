var MTAKPBS = {};

class ProjeTeklifIslemTuru {
    static RED = 1015;
    static ONAY = 1020;
    static REVIZE = 1025;
}

class ProjeIslemTuru {
    static KYTEKLND = 3200;
    static KYTGNCLLND = 3201;
    static KYTSLME = 3202;
    static DRMGNCLLME = 3203;
    static KYTDOGRLAMA = 3204;
    static GRSBLRTME = 3205;
    static RVZTLBI = 3206;
    static ONAYLAMA = 3207;
    static REDDETME = 3208;
    static ISKALEMIEKLENDI = 3209;
    static HRCMKLMEKLENDI = 3210;
    static FAALYTLRGUNCLLEND = 3211;
}


/**
 * Indicator utility for buttons
 */
class ButtonIndicator {
    static toggle(buttonId, isLoading) {
        if (isLoading) {
            ButtonIndicator.on(buttonId);
        } else {
            ButtonIndicator.off(buttonId);
        }
    }

    /**
     * Belirtilen ID'ye sahip button'ý DOM'da veya iframe'lerde bulur
     * @param {string} buttonId - The ID of the button without #
     * @returns {HTMLElement|null} - Bulunan button elementi veya null
     */
    static findButton(buttonId) {
        // 1. Önce ana dökümanda ara
        let button = document.getElementById(buttonId);
        if (button) return button;

        // 2. Ana dökümanda yoksa iframe'leri kontrol et
        const iframes = document.getElementsByTagName('iframe');
        for (let i = 0; i < iframes.length; i++) {
            try {
                const iframeDoc = iframes[i].contentDocument || iframes[i].contentWindow.document;
                if (iframeDoc) {
                    button = iframeDoc.getElementById(buttonId);
                    if (button) return button;
                }
            } catch (e) {
                // Cross-origin veya baþka bir hata olursa atla
                console.warn(`Iframe ${i} içeriðine eriþilemedi:`, e);
            }
        }

        // Hiçbir yerde bulunamazsa null döndür
        return null;
    }

    /**
     * Turn on loading indicator for a button
     * @param {string} buttonId - The ID of the button without #
     */
    static on(buttonId) {
        const button = ButtonIndicator.findButton(buttonId);
        if (button) {
            button.setAttribute('data-kt-indicator', 'on');
            button.setAttribute('disabled', 'disabled');
            button.classList.add('disabled');
        } else {
            console.warn(`Button with ID "${buttonId}" not found in DOM or iframes.`);
        }
    }

    /**
     * Turn off loading indicator for a button
     * @param {string} buttonId - The ID of the button without #
     */
    static off(buttonId) {
        const button = ButtonIndicator.findButton(buttonId);
        if (button) {
            button.setAttribute('data-kt-indicator', 'off');
            button.removeAttribute('disabled');
            button.classList.remove('disabled');
        } else {
            console.warn(`Button with ID "${buttonId}" not found in DOM or iframes.`);
        }
    }
}