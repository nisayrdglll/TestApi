var Application = {
    _activePageId: "",
    initialize: function () {
        var _this = Application;

        $("#page-tab-close-all-link").off("click").on("click", function () {
            _this.doCloseAllTabsLinkClick(this);
        });

        _this.initializePageLinks(document.getElementById("kt_app_body"));
    },
    openPage: function (pageId, pageURL) {
        var _this = Application;
        var pageTabElement = document.getElementById("page-tab-" + pageId);

        if (pageTabElement) {
            _this.activatePage(pageId);
        } else {
            _this.createPageTab(pageId);
            _this.createPageIframe(pageId, pageURL);
            _this._activePageId = pageId;
        }
    },
    createPageTab: function (pageId) {
        var _this = Application;
        var templateHTML = document.getElementById("template-page-tab").innerHTML;
        templateHTML = templateHTML.replace(/{{pageId}}/gi, pageId);
        document.getElementById("page-tabs-container").innerHTML += templateHTML;

        $("#page-tabs-container .page-tab-link").removeClass("active");
        $("#page-tab-link-" + pageId).addClass("active");

        $("#page-tabs-container .page-tab-link").off("click").on("click", function () {
            _this.doPageTabLinkClick(this);
        });

        $("#page-tabs-container .page-tab-close-link").off("click").on("click", function () {
            _this.doPageCloseLinkClick(this);
        });
    },
    createPageIframe: function (pageId, pageURL) {
        var _this = Application;
        var templateHTML = document.getElementById("template-page-iframe").innerHTML;
        templateHTML = templateHTML.replace(/{{pageId}}/gi, pageId);
        templateHTML = templateHTML.replace(/{{pageURL}}/gi, pageURL);

        $("#page-iframe-container .page-iframe").removeClass("active");

        var containerDIV = document.createElement("div");
        containerDIV.innerHTML = templateHTML;

        document.getElementById("page-iframe-container").appendChild(containerDIV);

        $("#page-iframe-" + pageId).on("load", function () {
            _this.doPageIframeLoad(this);
        });

        document.getElementById("page-iframe-" + pageId).src = pageURL;

        $("#page-iframe-" + pageId).addClass("active");
    },
    openDialog: function (dialogId, dialogURL, classCSS) {
        var _this = Application;
        var dialogElement = _this._pageDialogElement;
        classCSS = ((classCSS != undefined) ? classCSS : "");

        if (null == dialogElement) {
            return;
        }

        if (!dialogElement.hasAttribute("default-class")) {
            dialogElement.setAttribute("default-class",
                    dialogElement.getAttribute("class"));
        }

        if (classCSS != "") {
            dialogElement.setAttribute("class",
                    (dialogElement.getAttribute("default-class")
                    + " " + classCSS));
        }

        _this.createDialogIframe(dialogId, dialogURL);

        $(dialogElement).modal("show");
    },
    createDialogIframe: function (dialogId, dialogURL) {
        var _this = Application;
        var templateHTML = document.getElementById("template-dialog-iframe").innerHTML;
        templateHTML = templateHTML.replace(/{{dialogId}}/gi, dialogId);
        templateHTML = templateHTML.replace(/{{dialogURL}}/gi, dialogURL);

        document.getElementById("dialog-modal-body").innerHTML = templateHTML;

        $("#dialog-iframe-" + dialogId).on("load", function () {
            _this.doDialogIframeLoad(this);
        });

        document.getElementById("dialog-iframe-" + dialogId).src = dialogURL;
        document.getElementById("dialog-iframe-" + dialogId).classList.add("active");
    },
    activatePage: function (pageId) {
        var _this = Application;
        var currentActivePageId = document.body.getAttribute("data-active-page-id");

        if (undefined == document.getElementById("page-iframe-" + pageId)) {
            return;
        }

        if (pageId == currentActivePageId) {
            return;
        }

        _this._activePageId = pageId;
        document.body.setAttribute("data-active-page-id", pageId);
        
        // Activate page tab
        $("#page-tabs-container .page-tab-link.active").removeClass("active");
        $("#page-tab-link-" + pageId).addClass("active");

        // Activate page iframe
        $("#page-iframe-container .page-iframe.active").removeClass("active");
        $("#page-iframe-" + pageId).addClass("active");
    },
    setTabTitle: function (pageId, title) {
        var titleElement = document.getElementById("page-tab-title-" + pageId);
        if (titleElement) {
            titleElement.innerHTML = title;
        }
    },
    setDialogTitle: function (dialogId, title) {
        var titleElement = document.getElementById("dialog-modal-title");
        if (titleElement) {
            titleElement.innerHTML = title;
        }
    },
    doPageIframeLoad: function(sender) {
        var _this = Application;
        var pageId = sender.getAttribute("data-iframe-page-id");
        var iframeWindow = (sender.contentWindow || sender);
        var iframeDocument = (sender.contentDocument || sender.contentWindow.document);
        sender.style.height = iframeDocument.body.scrollHeight + 'px';

        if (iframeWindow.document) {
            _this.setTabTitle(
                    pageId,
                    iframeWindow.document.title);
        }

        _this.activatePage(_this._activePageId);
    },
    doDialogIframeLoad: function(sender) {
        var _this = Application;
        var dialogId = sender.getAttribute("data-iframe-dialog-id");
        var iframeWindow = (sender.contentWindow || sender);

        if (iframeWindow.document) {
            _this.setDialogTitle(
                    dialogId,
                    iframeWindow.document.title);
        }
    },
    doPageTabLinkClick: function(sender) {
        var _this = Application;
        var pageId = sender.getAttribute("data-tab-page-id");
        _this.activatePage(pageId);
    },
    initializePageLinks: function(parent) {
        var _this = Application;
        var elements = $(".page-menu-link", parent);
        var elementCount = elements.length;
        var element = null;
        var pageId = "";

        for (var i = 0; i < elementCount; i++) {
            element = elements[i];
            element.setAttribute("data-href", element.href);
            element.href = "JavaScript:void(0);";

            $(element).off("click").on("click", function () {
                _this.doPageMenuLinkClick(this);
            });
        }

        elements = $(".page-menu-link.initial-page-menu-link", parent);

        if (elements.length > 0) {
            element = elements[0];
            pageId = element.getAttribute("data-menu-page-id");
            _this.openPage(element.getAttribute("data-menu-page-id"),
                    element.getAttribute("data-href"));
        }
    },
    doPageMenuLinkClick: function(sender) {
        var _this = Application;
        var pageId = sender.getAttribute("data-menu-page-id");
        var pageURL = sender.getAttribute("data-href");

        _this.openPage(pageId, pageURL);
    },
    doPageRefreshLinkClick: function(sender) {
        var _this = Application;
        var pageId = sender.getAttribute("data-tab-page-id");

        var iframeElement = document.getElementById("page-iframe-" + pageId);

        if (iframeElement) {
            iframeElement.src = iframeElement.src;
        }

        _this.activatePage(pageId);
    },
    doPageCloseLinkClick: function(sender) {
        var _this = Application;
        var pageId = sender.getAttribute("data-tab-page-id");
        var currentPageId = document.body.getAttribute("data-active-page-id");
        var tabs = document.getElementById("page-tabs-container").children;
        var tabCount = tabs.length;
        var exit = false;
        var tab = null;
        var tabIndex = 0;

        for (var i = 0; ((i < tabCount) && (!exit)); i++) {
            tab = tabs[i];

            if (tab.id == ("page-tab-" + pageId)) {
                exit = true;
                tabIndex = (i - 1);
            }
        }

        if (tabIndex < 0) {
            tabIndex = 0;
        }

        $("#page-iframe-" + pageId).detach();
        $("#page-tab-" + pageId).detach();

        if (currentPageId == pageId) {
            document.body.setAttribute("data-active-page-id", "");
        }

        tabCount--;

        if (tabCount > 0) {
            tab = tabs[tabIndex];
            pageId = tab.getAttribute("data-tab-page-id");
            _this.activatePage(pageId);
        }
    },
    doCloseAllTabsLinkClick: function(sender) {
        document.getElementById("page-tabs-container").innerHTML = "";
        document.getElementById("page-iframe-container").innerHTML = "";
        document.body.setAttribute("data-active-page-id", "");
    }
}

Application.initialize();