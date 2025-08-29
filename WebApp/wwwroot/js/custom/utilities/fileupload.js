// fileupload.js
const FileUploader = (function () {
    class FileUploaderClass {
        constructor(module, recordId) {
            this.module = module;
            this.recordId = recordId;
            this.initializeEventListeners();
            this.loadExistingFiles();
            this.initializeModal();
        }

        initializeEventListeners() {
            const dropZone = document.getElementById('drop-zone');
            const fileInput = document.getElementById('file-input');

            ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
                dropZone.addEventListener(eventName, this.preventDefaults, false);
            });

            ['dragenter', 'dragover'].forEach(eventName => {
                dropZone.addEventListener(eventName, () => {
                    dropZone.querySelector('.border-dashed').classList.add('bg-primary-light');
                });
            });

            ['dragleave', 'drop'].forEach(eventName => {
                dropZone.addEventListener(eventName, () => {
                    dropZone.querySelector('.border-dashed').classList.remove('bg-primary-light');
                });
            });

            dropZone.addEventListener('drop', (e) => this.handleFiles(e.dataTransfer.files));
            fileInput.addEventListener('change', (e) => this.handleFiles(e.target.files));
        }

        preventDefaults(e) {
            e.preventDefault();
            e.stopPropagation();
        }

        handleFiles(files) {
            [...files].forEach(file => this.uploadFile(file));
        }

        uploadFile(file) {
            const progressDiv = document.createElement('div');
            progressDiv.className = 'd-flex align-items-center py-3';
            progressDiv.innerHTML = `
                <div class="d-flex align-items-center flex-grow-1 me-2">
                    <span class="svg-icon svg-icon-2x svg-icon-primary me-4">
                        <i class="bi bi-file-earmark-text fs-2 text-primary"></i>
                    </span>
                    <div class="d-flex flex-column">
                        <span class="fs-6 fw-bolder text-dark">${file.name}</span>
                        <span class="fs-7 text-muted percent">0%</span>
                    </div>
                </div>
                <div class="progress h-6px w-100 ms-10">
                    <div class="progress-bar bg-primary" role="progressbar" style="width: 0%" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
            `;
            document.getElementById('file-list').appendChild(progressDiv);

            const formData = new FormData();
            formData.append('file', file);
            formData.append('module', this.module);
            formData.append('recordId', this.recordId);

            $.ajax({
                url: '/Upload/FileUpload',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                xhr: () => {
                    const xhr = new XMLHttpRequest();
                    xhr.upload.addEventListener('progress', (e) => {
                        if (e.lengthComputable) {
                            const percentComplete = (e.loaded / e.total) * 100;
                            const progress = progressDiv.querySelector('.progress-bar');
                            const percentSpan = progressDiv.querySelector('.percent');

                            progress.style.width = percentComplete + '%';
                            progress.setAttribute('aria-valuenow', percentComplete);
                            percentSpan.textContent = Math.round(percentComplete) + '%';
                        }
                    });
                    return xhr;
                },
                success: () => {
                    progressDiv.classList.add('card-flush');
                    setTimeout(() => {
                        progressDiv.remove();
                        this.loadExistingFiles();
                    }, 1000);
                    toastr.success('Dosya başarıyla yüklendi');
                },
                error: (error) => {
                    // Remove progress bar when error occurs
                    progressDiv.querySelector('.progress').remove();
                    
                    progressDiv.innerHTML = `
                        <div class="d-flex w-100 p-4 bg-light-danger rounded border border-danger">
                            <span class="svg-icon svg-icon-2x me-4">
                                <i class="bi bi-exclamation-circle fs-2 text-danger"></i>
                            </span>
                            <div class="d-flex flex-column flex-grow-1">
                                <span class="fs-6 fw-bold text-dark text-truncate">${file.name}</span>
                                <span class="fs-7 text-danger mt-1">Yükleme başarısız: ${error.responseText}</span>
                            </div>
                        </div>
                    `;
                    toastr.error('Dosya yüklenirken hata oluştu');
                }
            });
        }

        loadExistingFiles() {
            $.ajax({
                url: '/Upload/List',
                type: 'GET',
                data: {
                    module: this.module,
                    recordId: this.recordId
                },
                success: (files) => {
                    const tbody = document.querySelector('#uploaded-files-table tbody');
                    tbody.innerHTML = files.map(file => `
                        <tr>
                            <td>
                                <div class="d-flex align-items-center">
                                    <i class="ki-duotone ki-file-down fs-2x text-primary me-4"></i>
                                    <span class="fw-bold text-gray-800">${file.adi}</span>
                                </div>
                            </td>
                             <td>
                                <div class="d-flex align-items-center">
                                    <span class="fw-bold text-gray-800">${file.aciklama || ''}</span>
                                </div>
                            </td>
                            <td>${this.formatFileSize(file.boyut)}</td>
                            <td>${this.formatDate(new Date(file.createdAt))}</td>
                            <td>
                                <button class="btn btn-sm btn-icon btn-light-success me-1" onclick="fileUploaderInstance.showEditModal(${file.id}, '${file.adi}', '${file.aciklama || ''}')" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-dark" data-bs-placement="top" title="Düzenle">
                                    <i class="ki-outline ki-pencil fs-2"></i>
                                </button>
                                <button class="btn btn-sm btn-icon btn-light-primary me-1" onclick="fileUploaderInstance.downloadFile(${file.id})" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-dark" data-bs-placement="top" title="İndir" data-kt-indicator="off">
                                    <span class="indicator-label">
                                        <i class="ki-duotone ki-file-down fs-2">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm"></span>
                                    </span>
                                </button>
                                <button class="btn btn-sm btn-icon btn-light-danger" onclick="fileUploaderInstance.deleteFile(${file.id})" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-dark" data-bs-placement="top" title="Sil" data-kt-indicator="off">
                                    <span class="indicator-label">
                                        <i class="ki-duotone ki-trash fs-2">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                            <span class="path3"></span>
                                            <span class="path4"></span>
                                            <span class="path5"></span>
                                        </i>
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm"></span>
                                    </span>
                                </button>
                            </td>
                        </tr>
                    `).join('');
                },
                error: () => {
                    toastr.error('Dosyalar yüklenirken hata oluştu');
                }
            });
        }

        downloadFile(id) {
            const downloadUrl = `/Upload/Download/${id}?module=${this.module}`;
            const button = document.querySelector(`button[onclick="fileUploaderInstance.downloadFile(${id})"]`);
            
            if (button.getAttribute('data-kt-indicator') === 'on') {
                return;
            }

            button.setAttribute('data-kt-indicator', 'on');
            
            // Önce dosyanın var olduğunu kontrol edelim
            $.ajax({
                url: downloadUrl,
                type: 'HEAD',
                success: () => {
                    // Dosya varsa, gizli bir iframe ile indirelim
                    const iframe = document.createElement('iframe');
                    iframe.style.display = 'none';
                    iframe.src = downloadUrl;
                    document.body.appendChild(iframe);
                    
                    // İndirme başladıktan 2 saniye sonra indicator'ü kaldıralım
                    setTimeout(() => {
                        button.setAttribute('data-kt-indicator', 'off');
                        document.body.removeChild(iframe);
                    }, 2000);
                },
                error: () => {
                    toastr.error('Dosya bulunamadı');
                    button.setAttribute('data-kt-indicator', 'off');
                }
            });
        }

        deleteFile(id) {
            const button = document.querySelector(`button[onclick="fileUploaderInstance.deleteFile(${id})"]`);
            
            if (button.getAttribute('data-kt-indicator') === 'on') {
                return;
            }

            Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu dosyayı silmek istediğinize emin misiniz?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Evet, sil!',
                cancelButtonText: 'İptal',
                customClass: {
                    confirmButton: "btn btn-danger",
                    cancelButton: 'btn btn-light'
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    button.setAttribute('data-kt-indicator', 'on');
                    
                    $.ajax({
                        url: `/Upload/Delete/${id}?module=${this.module}`,
                        type: 'DELETE',
                        success: () => {
                            toastr.success('Dosya başarıyla silindi');
                            this.loadExistingFiles();
                        },
                        error: (xhr) => {
                            toastr.error('Dosya silinirken bir hata oluştu');
                            button.setAttribute('data-kt-indicator', 'off');
                        }
                    });
                }
            });
        }

        initializeModal() {
            // Önce modal style'ını ekleyelim
            const style = document.createElement('style');
            style.textContent = `
                .indicator-progress {
                    display: none !important;
                }
                [data-kt-indicator="on"] .indicator-label {
                    display: none !important;
                }
                [data-kt-indicator="on"] .indicator-progress {
                    display: flex !important;
                }
            `;
            document.head.appendChild(style);

            const modalHtml = `
                <div class="modal fade" id="editFileModal" tabindex="-1" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered mw-650px">
                        <div class="modal-content">
                            <div class="modal-header pb-0 border-0 justify-content-end">
                                <div class="btn btn-sm btn-icon btn-active-color-primary" data-bs-dismiss="modal">
                                    <i class="ki-outline ki-cross fs-1"></i>
                                </div>
                            </div>
                            <div class="modal-body mx-5 mx-xl-15 my-7">
                                <div class="scroll-y me-n7 pe-7" data-kt-scroll="true" data-kt-scroll-activate="{default: false, lg: true}" data-kt-scroll-max-height="auto" data-kt-scroll-dependencies="#kt_modal_add_customer_header" data-kt-scroll-wrappers="#kt_modal_add_customer_scroll" data-kt-scroll-offset="300px">
                                    <div class="notice d-flex bg-light-primary rounded border-primary border border-dashed mb-9 p-6">
                                        <i class="ki-duotone ki-pencil fs-2tx text-primary me-4">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                        <div class="d-flex flex-stack flex-grow-1">
                                            <div class="fw-semibold">
                                                <h4 class="text-gray-900 fw-bold">Dosya Düzenle</h4>
                                                <div class="fs-6 text-gray-700">Dosya adını ve açıklamasını düzenleyebilirsiniz.</div>
                                            </div>
                                        </div>
                                    </div>
                                    <input type="hidden" id="editFileId">
                                    <div class="fv-row mb-7">
                                        <label class="required fw-semibold fs-6 mb-2">Dosya Adı</label>
                                        <div class="input-group input-group-solid">
                                            <span class="input-group-text">
                                                <i class="ki-duotone ki-document fs-3">
                                                    <span class="path1"></span>
                                                    <span class="path2"></span>
                                                </i>
                                            </span>
                                            <input type="text" class="form-control form-control-solid" id="editFileName" />
                                        </div>
                                    </div>
                                    <div class="fv-row mb-7">
                                        <label class="fw-semibold fs-6 mb-2">Açıklama</label>
                                        <div class="input-group input-group-solid">
                                            <span class="input-group-text">
                                                <i class="ki-duotone ki-message-text fs-3">
                                                    <span class="path1"></span>
                                                    <span class="path2"></span>
                                                </i>
                                            </span>
                                            <textarea class="form-control form-control-solid" id="editFileDescription" rows="4"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer flex-center">
                                <button type="button" class="btn btn-light-danger btn-sm me-3" data-bs-dismiss="modal">
                                    <i class="ki-duotone ki-cross-circle fs-2 me-1">
                                        <span class="path1"></span>
                                        <span class="path2"></span>
                                    </i>
                                    <span>İptal</span>
                                </button>
                                <button type="button" class="btn btn-light-primary btn-sm" id="saveFileEdit" data-kt-indicator="off">
                                    <span class="indicator-label d-flex align-items-center">
                                        <i class="ki-duotone ki-check-circle fs-2 me-2">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                        <span class="fs-7 fw-bold">Kaydet</span>
                                    </span>
                                    <span class="indicator-progress d-flex align-items-center">
                                        <span class="spinner-border spinner-border-sm align-middle me-2"></span>
                                        <span class="fs-7 fw-bold">Lütfen bekleyin...</span>
                                    </span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>`;
            document.body.insertAdjacentHTML('beforeend', modalHtml);

            const modal = document.getElementById('editFileModal');
            modal.addEventListener('hidden.bs.modal', () => {
                const saveButton = document.getElementById('saveFileEdit');
                saveButton.removeAttribute('data-kt-indicator');
                saveButton.disabled = false;
            });

            document.getElementById('saveFileEdit').addEventListener('click', (e) => {
                const button = document.getElementById('saveFileEdit');
                
                // Eğer buton zaten loading durumundaysa işlemi engelle
                if (button.getAttribute('data-kt-indicator') === 'on') {
                    return;
                }

                button.setAttribute('data-kt-indicator', 'on');
                button.disabled = true;

                this.saveFileEdit()
                    .then(() => {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('editFileModal'));
                        if (modal) {
                            modal.hide();
                        }
                        this.loadExistingFiles();
                        toastr.success('Dosya başarıyla güncellendi');
                    })
                    .catch(() => {
                        toastr.error('Dosya güncellenirken hata oluştu');
                    })
                    .finally(() => {
                        button.setAttribute('data-kt-indicator', 'off');
                        button.disabled = false;
                    });
            });
        }

        saveFileEdit() {
            const fileId = document.getElementById('editFileId').value;
            const fileName = document.getElementById('editFileName').value;
            const fileDescription = document.getElementById('editFileDescription').value;

            return $.ajax({
                url: '/Upload/UpdateFile',
                type: 'POST',
                data: {
                    id: fileId,
                    module: this.module,
                    filename: fileName,
                    description: fileDescription
                }
            });
        }

        showEditModal(id, name, description = '') {
            const modal = document.getElementById('editFileModal');
            const saveButton = document.getElementById('saveFileEdit');
            
            document.getElementById('editFileId').value = id;
            document.getElementById('editFileName').value = name;
            document.getElementById('editFileDescription').value = description;
            
            saveButton.removeAttribute('data-kt-indicator');
            saveButton.disabled = false;
            
            new bootstrap.Modal(modal).show();
        }

        formatDate(date) {
            const pad = (num) => String(num).padStart(2, '0');
            
            const year = date.getFullYear();
            const month = pad(date.getMonth() + 1);
            const day = pad(date.getDate());
            const hours = pad(date.getHours());
            const minutes = pad(date.getMinutes());
            const seconds = pad(date.getSeconds());
            
            return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
        }

        formatFileSize(bytes) {
            if (bytes === 0) return '0 Bytes';
            const k = 1024;
            const sizes = ['Bytes', 'KB', 'MB', 'GB'];
            const i = Math.floor(Math.log(bytes) / Math.log(k));
            return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
        }
    }

    return {
        init: function (module, recordId) {
            window.fileUploaderInstance = new FileUploaderClass(module, recordId);
            return window.fileUploaderInstance;
        }
    };
})();

// ES6 module export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = FileUploader;
}