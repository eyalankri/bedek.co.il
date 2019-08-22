<template>
  <div class="handyman">
    <div class="container">
      <div style="100%; max-width:600px">
        <vue-good-table
          @on-selected-rows-change="selectionChanged"
          :columns="columns"
          :rows="rows"
          :search-options="{ enabled: true, placeholder: ' חפש בטבלה ', }"
          :rtl="true"
          :pagination-options="{ enabled: true, perPage: 50 , perPageDropdown: [100]}"
          styleClass="vgt-table condensed"
          :selectOptions="{
           enabled: true,
          selectOnCheckboxOnly: false, // only select when checkbox is clicked instead of the row
          selectionInfoClass: 'custom-class',
          selectionText: 'חוקי מכר נבחרו',
          clearSelectionText: '',
          disableSelectInfo: true, // disable the select info panel on top
  }"
        >
          >
          <div slot="emptystate">אין נתונים בטבלה</div>

          <template slot="table-row" slot-scope="props">
            <!--  if col name is 'isChecked' -->
            <span v-if="props.column.field == 'isChecked'">
              <input :checked="props.row.isChecked" type="checkbox" />
              <span></span>
            </span>
            <span v-else>{{props.formattedRow[props.column.field]}}</span>
          </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "serviceInHandyman",
  components: { VueGoodTable },

  data() {
    return {
      handymanId: this.$route.params.handymanId,
      serviceName: null,
      services: [],
      allowUpdate: false,
      rows: [],
      columns: [
        {
          label: "id",
          field: "serviceId",
          type: "number",
          hidden: true
        },
        {
          label: "חוק המכר",
          field: "serviceName"
        },
        {
          label: "",
          field: "isChecked",
          html: true
        }
      ]
    };
  },

  mounted() {
    if (this.handymanId != null) {
      this.loadHandymanInfo();
      this.listServices();
    }
  },
  methods: {
    selectionChanged(params) {
      
       
      if (!this.allowUpdate) {
        // when page is loading there is a row selected which calls selectionChanged() which initiate the update.
        // allowUpdate false will block it. only than set to true
        this.allowUpdate = true;
        return false;
      }
      this.updateServiceInHandyMan(params);
    },
    loadHandymanInfo() {
      // also get the building info
      axios
        .get(
          process.env.ROOT_API +
            "ServiceInHandyman/Get?handymanId=" +
            this.handymanId,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          this.progressBar = false;

          this.$store.commit(
            "setInfoBarText",
            `שם איש המקצוע: ${res.data.firstName} ${res.data.lastName}`
          );
        })
        .catch(error => {
          console.log(error);
        });
    },
    updateServiceInHandyMan(params) {
      
     console.clear();

      var listDto = [];
      var userId = this.handymanId;
      console.log(params.selectedRows.length)
      if (params.selectedRows.length == 0) { // for remove all checkboxex
       let obj = {
          UserId: userId,
          ServiceId: 0,
          RemoveAll: true
        };
        listDto.push(obj); 
      };
      params.selectedRows.forEach(function(el) {
        let obj = {
          UserId: userId,
          ServiceId: el.serviceId,
          RemoveAll: false,
        };
        listDto.push(obj);
      });
     console.log(listDto)
      

      axios
        .post(
          process.env.ROOT_API + "ServiceInHandyman/Update",
          listDto,
          this.$store.getters.getTokenHeader
        )
        .then(response => {
          this.listServices();
        })
        .catch(error => {
          console.log(error);
        });
    },
    listServices() {
      axios
        .get(
          process.env.ROOT_API +
            "ServiceInHandyman/List?handymanId=" +
            this.handymanId,
          this.$store.getters.getTokenHeaderFormData
        )
        .then(response => {
          var cnt = 0;
          response.data.forEach(el => {
            el.rowId = cnt;
            cnt++;

            el.isChecked = el.userId != null ? true : false;
          });

          this.rows = response.data;

          this.allowUpdate = true; 
          this.rows.forEach(el => {
            if (el.isChecked) {
              this.$set(this.rows[el.rowId], "vgtSelected", true);
              this.allowUpdate = false; // if there are selected rows set allowUpdate=false // because in it will post to the server before any manulally selection
            }
          });
    
          
        })
        .catch(error => {
          console.log(error);
        });
    }
  }
};
</script>
 