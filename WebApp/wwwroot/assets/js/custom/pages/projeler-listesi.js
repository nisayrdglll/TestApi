var KOSGEBIDSCustomPage = {
  initialize: function () {
    var _this = KOSGEBIDSCustomPage;

    _this.createDataTable();
  },
  createDataTable: function () {
    $.getJSON("data/projeler-listesi.json", function (data) {
      var table = $("#table-proje-listesi").DataTable({
        data: data,
        columns: [
          { data: "ResultId", title: "ID" }, // ID sütunu
          { data: "Code", title: "Proje Kodu" }, // Proje Kodu sütunu
          {
            data: "Name",
            render: function (data, type, full, meta) {
              return `<a class="menu-link page-menu-link" href="program-detay.html" data-menu-page-id="program-detay">${full.ShortName}
                                        <span class="text-gray-500 fw-semibold fs-7 d-block text-start ps-0">${full.Name}</span>
                                    </a>`;
            },
            title: "Proje Adı", // Proje Adı sütunu
          },
          {
            data: "Status",
            render: function (data, type, full, meta) {
              if (data === "Hazırlık Aşaması") {
                return '<span class="badge badge-secondary">Hazırlık Aşaması</span>';
              } else if (data === "Devam Ediyor") {
                return '<span class="badge badge-primary">Devam Ediyor</span>';
              } else if (data === "Tamamlandı") {
                return '<span class="badge badge-success">Tamamlandı</span>';
              } else if (data === "İptal Edildi") {
                return '<span class="badge badge-danger">İptal Edildi</span>';
              } else {
                return "-";
              }
            },
            title: "Durum", // Durum sütunu
          },
          { data: "StartDate", title: "Başlangıç Tarihi" }, // Başlangıç Tarihi sütunu
          { data: "EndDate", title: "Bitiş Tarihi" }, // Bitiş Tarihi sütunu
          {
            data: null,
            title: "İşlemler",
            render: function (data, type, full, meta) {
              return `<button class="btn btn-primary" onclick="window.location.href='proje-detay-list.html'">Detay</button>`;
            },
            orderable: false, // Bu sütunda sıralama yapılmaması için
          },
        ],
        order: [[1, "asc"]], // Proje Kodu'na göre sıralama
        searching: true, // Arama özelliği
        paging: true, // Sayfalama
        info: true, // Tablo bilgisi
      });

      // Arama kutusu ile arama
      $("#search-input").on("keyup", function () {
        table.search(this.value).draw();
      });

      KOSGEBIDSPage.initializePageLinks(
        document.getElementById("table-proje-listesi")
      );
    });
  },
};

window.addEventListener("load", function () {
  KOSGEBIDSCustomPage.initialize();
});
