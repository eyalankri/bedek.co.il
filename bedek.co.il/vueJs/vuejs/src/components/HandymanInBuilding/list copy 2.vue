<template>
  <div class="ServiceInHandymanInBuilding">
    <div class="container">
      <table id="ServiceInHandymanInBuilding" class="mdl-data-table" width="100%"></table>
    </div>
    <div style="padding:5%">
      <vue-good-table
        @on-selected-rows-change="selectionChanged"
        :columns="columns"
        :rows="rows"
        :select-options="{ enabled: true }"
        :search-options="{ enabled: true }"
        :row-style-class="rowStyleClassFn"
        :rtl="true"
      >
        <div slot="emptystate">אין בניינים ברשימה</div>
      </vue-good-table>
      <!-- click on a row below to show the action button -->
    </div>
  </div>
</template>

<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "handymanInBuilding",
  components: { VueGoodTable },
  data() {
    return {
      buildingId: this.$route.params.buildingId,
      street: null,
      buildingNumber: null,
      projectName: null,
      city: null,
      dataset: [],

      columns: [
        {
          label: 'חוק המכר',
          field: 'serviceName',                   
         },
        {
          label: 'שם',
          field: 'firstName',                   
         },
        {
          label: "LastName",
          field: "lastName"
        },
        {
          label: "Company",
          field: "company",
          
        },
        {
          label: "BuildingId",
          field: "buildingId",
          type: "number",          
        },
        {
           label: 'בחר',
          field: 'chk',
          html: true,    
        }
      ],
      rows: []
    };
  },
  mounted() {
    this.loadBuilding();
    this.loadServiceInHandymanInBuilding();

    this.rows.forEach(function(row) {
      if (row.name == "Dan") {                        
        $('.clsRow_Dan  input[type="checkbox"]').click();
      }
    });
  },

  methods: {
    rowStyleClassFn(row) {
      return row.name == "Dan" ? "clsRow_" + row.name : "";
    },
    selectionChanged(params) {
      var arr = params.selectedRows;
      arr.forEach(function(element) {
        console.log(element.name, element.age);
      });
    },
    loadBuilding() {
      axios
        .get(
          process.env.ROOT_API + "Building/Get?buildingId=" + this.buildingId,
          this.$store.getters.getTokenHeaderFormData
        )
        .then(res => {
          this.projectName = res.data.projectName;
          this.city = res.data.city;
          this.street = res.data.street;
          this.buildingNumber = res.data.buildingNumber;
          this.$store.commit(
            "setInfoBarText",
            `שיוך אנשי מקצוע: ${this.projectName} ${this.buildingNumber} - ${this.city}`
          );
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadServiceInHandymanInBuilding() {
      this.rows = [];
      axios
        .get(
          process.env.ROOT_API +
            "ServiceInHandymanInBuilding/List?buildingId=" +
            this.buildingId,
          this.$store.getters.getTokenHeaderFormData
        )
        .then(response => {
          response.data.forEach(el => {
            console.log(el);
            this.rows.push([
              //el.UserId,
                el.firstName,
                el.lastName,
                el.company,
                el.serviceName,
                el.buildingId,                                       
              `<label>
                    <input type="checkbox" class="abc"><span></span>
                  </label>`
            ]);
          });

          this.initializeDataTable();
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
        });
    },
    initializeDataTable() {
      // create & bind the data.
      $("#ServiceInHandymanInBuilding").DataTable({
        data: this.rows,
        language: {
          processing: "מעבד...",
          lengthMenu: "הצג _MENU_ פריטים",
          zeroRecords: "לא קיימות דירות לבניין זה",
          emptyTable: "לא קיימות דירות לבניין זה",
          info: "_START_ עד _END_ מתוך _TOTAL_ רשומות",
          infoEmpty: "0 עד 0 מתוך 0 רשומות",
          infoFiltered: "(מסונן מסך _MAX_  רשומות)",
          infoPostFix: "",
          search: "חפש:",
          url: "",
          paginate: {
            first: "ראשון1",
            previous: "קודם",
            next: "הבא",
            last: "אחרון"
          }
        },
        order: [0, "desc"],
        columns: [
          { title: "סעיף מכר" },
          { title: "חברה" },
          { title: "שם" },
          { title: "משפחה" },
          { title: "" }
        ],
        columnDefs: [
          {
            //targets: [0,1, 2, 3, 4],
            className: "mdl-data-table__cell--non-numeric"
          },
          {
            // first col
            targets: [0],
            visible: true,
            searchable: true
          }
        ]
      });

      $(".mdl-cell--6-col:first").attr(
        "class",
        "mdl-cell--12-col-phone mdl-cell--2-col"
      );
      $(".mdl-cell--6-col").attr(
        "class",
        "mdl-cell--12-col-phone mdl-cell--10-col"
      );
    }
  }
};
$(function() {
  $(".abc").change(function() {
    alert(123);
  });
});
</script>

<style>
.vgt-checkbox-col input[type="checkbox"] {
  opacity: 1;
}
</style>
