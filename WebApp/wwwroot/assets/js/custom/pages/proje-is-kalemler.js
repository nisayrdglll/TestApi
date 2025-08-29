var KOSGEBIDSCustomPage = {
  initialize: function () {
    var _this = KOSGEBIDSCustomPage;

    _this.createDataTable();
  },
  createDataTable: function () {
    $.getJSON("data/proje-is-kalemler.json", function (data) {
      var table = $("#table-proje-is-kalemler-listesi").DataTable({
        data: data,
        columns: [
          { data: "ResultId", title: "ID" }, // ID sütunu
          { data: "ShortName", title: "Kısa Adı" }, // Proje Kodu sütunu
          {
            data: "Name",
            render: function (data, type, full, meta) {
              return `<a class="menu-link page-menu-link" href="program-detay.html" data-menu-page-id="program-detay">${full.ShortName}
                                        <span class="text-gray-500 fw-semibold fs-7 d-block text-start ps-0">${full.Name}</span>
                                    </a>`;
            },
            title: "Adı", // Proje Adı sütunu
          },
          {
            data: "Status",
            render: function (data, type, full, meta) {
              if (data === "Evet") {
                return '<span class="badge badge-success">Evet</span>';
              } else if (data === "Hayır") {
                return '<span class="badge badge-danger">Hayır</span>';
              }
            },
            title: "Aktif", // Durum sütunu
          },
          {
            data: "UnitName",
            title: "Birim Adı",
          },
        ],
        order: [[0, "asc"]], // Proje Kodu'na göre sıralama
        searching: true, // Arama özelliği
        paging: true, // Sayfalama
        info: true, // Tablo bilgisi
      });

      // Arama kutusu ile arama
      $("#search-input").on("keyup", function () {
        table.search(this.value).draw();
      });

      KOSGEBIDSPage.initializePageLinks(
        document.getElementById("table-proje-is-kalemler-listesi")
      );
    });
  },
};

window.addEventListener("load", function () {
  KOSGEBIDSCustomPage.initialize();
});
