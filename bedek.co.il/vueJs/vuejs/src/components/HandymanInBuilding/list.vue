<template>
  <div class="ServiceInHandymanInBuilding">
    <div class="container">
     
      <vue-good-table
        @on-selected-rows-change="selectionChanged"
        :columns="columns"
        :rows="rows"
        :select-options="{ enabled: true }"
        :search-options="{ 
          enabled: true,
           placeholder: ' חפש בטבלה ', 
          }"        
        :rtl="true"
         styleClass="vgt-table condensed">
       
      
        <div slot="emptystate">אין בניינים ברשימה</div>
      </vue-good-table>
      
      <!-- click on a row below to show the action button -->
    </div>
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
      

      columns: [
        {
          label: "חוק המכר",
          field: "serviceName"
        },
        {
          label: "שם פרטי",
          field: "firstName"
        },
        {
          label: "שם משפחה",
          field: "lastName"
        },
        {
          label: "חברה",
          field: "company"
        },
        {
          field: "userId",
          hidden: true
        },
        {
          label: "בחר",
          field: "isAssociated",
          html: true
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

      $(".chkSelected").prop("checked", false);

      arr.forEach(function(el) {
        //console.log(el.userId);

        var chk = $("." + el.userId + "_" + el.serviceId);
        $(chk).prop("checked", true);
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
            el.isAssociated = `<input type="checkbox" ${
              el.isAssociated ? "checked" : ""
            } class="chkSelected ${el.userId}_${el.serviceId}"><span></span>`;
          });

          this.rows = response.data;
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
        });
    }
  }
};
</script>

<style>
.vgt-checkbox-col {
  display: none;
}
.vgt-checkbox-col input[type="checkbox"] {
  opacity: 1;
}
</style>
