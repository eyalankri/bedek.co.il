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
        :pagination-options="{ 
         enabled: true, 
         perPage: 10 , 
         perPageDropdown: [50, 100]
        }"
        styleClass="vgt-table condensed"
      >
        <div slot="emptystate">אין אנשי מקצוע ברשימה</div>
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
      allowUpdate: false,
      checkedItems: [],
      rows: [],
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
          field: "isAssociated",
          hidden: true
        },
        {
          field: "buildingId",
          hidden: true
        },
        {
          label: "בחר",
          field: "checkBox",
          html: true
        }
      ]
    };
  },
  mounted() {
    this.loadBuilding();
    this.loadServiceInHandymanInBuilding();
  },

  methods: {
    selectionChanged(params) {
      this.updateServiceInHandymanInBuilding(params);
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
            `שיוך חוקי מכר לבניין: ${this.projectName} ${this.buildingNumber} - ${this.city}`
          );
        })
        .catch(error => {
          console.log(error);
        });
    },
    updateServiceInHandymanInBuilding(params) {
      if (!this.allowUpdate) {
        // when page is loading there is a row selected which calls selectionChanged() which initiate the update.
        // allowUpdate false will block it. only than set to true
        this.allowUpdate = true;
        return false;
      }

      let buildingId = this.buildingId;
       
      $(".clsChk").prop("checked", false); // init un-check all

      var listDto = [];
      params.selectedRows.forEach(function(el) {
        var chkElm = $("#" + el.userId + "_" + el.serviceId);
        $(chkElm).prop("checked", true);

        var obj = {
          UserId: el.userId,
          FirstName: el.firstName,
          LastName: el.lastName,
          ServiceName: el.serviceName,
          ServiceId: el.serviceId,
          Company: el.company,
          BuildingId: buildingId
        };
        listDto.push(obj);
      });

      axios
        .post(
          process.env.ROOT_API + "ServiceInHandymanInBuilding/Update",
          listDto,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          console.log(res);
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadServiceInHandymanInBuilding() {
      console.clear();
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
            var isChecked = el.isAssociated ? "checked" : "";
            var id = `${el.userId}_${el.serviceId}`;
            el.checkBox = `<input type='checkbox' class='${isChecked} clsChk' id='${id}'><span></span>`;
            this.checkedItems.push(id);
          });

          this.rows = response.data;

          //this.$set(this.rows[0], 'vgtSelected', true);

          this.checkedItems.forEach(function(id) {
            $("#" + id).ready(function() {
              var el = $("#" + id);
              if ($(el).hasClass("checked")) {
                $(el).click();
              }
            });
          });
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
