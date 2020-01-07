<template>
  <div class="container">
    <div class="row">
      <div class="progress" v-if="progressBar" style="margin-top:30px;">
        <div class="indeterminate"></div>
      </div>
      <div id="vgt">
        <vue-good-table
          :columns="columns"
          :rows="rows"
          :rtl="true"
          :search-options="{ enabled: true,placeholder: ' חפש בטבלה ',}"
          :pagination-options="{ enabled: true, perPage: 10 , perPageDropdown: [50, 100]}"
          styleClass="vgt-table condensed"
        >
          <div slot="emptystate">אין נתונים בטבלה</div>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";
import moment from "moment";


export default {
  name: "ServiceCallList",
  components: {
    VueGoodTable
  },
  data() {
    return {
        progressBar: null,
      /*   Status = sc.Status,
                           ApartmentNumber = a.ApartmentNumber,
                           BuildingNumber = b.BuildingNumber,
                           ApartmentId = a.ApartmentId,
                           City = b.City,
                           Description = sc.Description,
                           FirstName = u.FirstName,
                           LastName = u.LastName    */
     rows: [],
      columns: [
        {
          label: "Id",
          field: "serviceCallId",
         hidden:true          
        },
         {
          label: "סטטוס",
          field: "status"
        },
          {
          label: "עיר",
          field: "city"
        },
         {
          label: "פרוייקט",
          field: "projectName"
        },
         {
          label: "דירה",
          field: "apartmentNumber"
        },
        {
          label: "משפחה",
          field: "lastName"
        },
        {
          label: "שם",
          field: "firstName"
        },
       
        {
          label: "ת.פתיחה",
          field: "dateCreated"
        },
        {
          label: "ת.עדכון",
          field: "dateUpdated"
        },
        
      

      ]
    };
  },
  mounted() {
       this.$store.commit(
            "setInfoBarText",
            `קריאות שירות`
          );
    this.loadServiceCalls();
  },
  methods: {
    loadServiceCalls() {
      this.progressBar = true;
    
      axios
        .get(
          process.env.ROOT_API + "ServiceCall/List/",
          this.$store.getters.getTokenHeader
        )
        .then(res => {
          console.log(res.data);
          res.data.forEach(res => {
            res.dateUpdated = moment(res.dateUpdated).format("DD/MM/YYYY");
            res.dateCreated = moment(res.dateCreated).format("DD/MM/YYYY");
          });

          this.rows = res.data;
          this.progressBar = null;
        })
        .catch(error => {
          console.log("loadBuildingInfo: " + error);
        });
    }
  }
};
</script>

<style>
</style>