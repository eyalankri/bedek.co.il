<template>
  <div class="handyman">
    <div class="container">
      <table id="services" class="mdl-data-table" width="75%"></table>
      <div class="row">
         <table style="width:75%" class="highlight">
           <thead>
             <tr>
               <th>שיוך אנשי מקצוע לחוקי מכר</th>
               <th></th>
             </tr>
           </thead>
           
           <tbody>
             <tr v-for="item in dataset" :key="item[0]" >               
               <td>{{item[1]}}</td>
               <td>
                  <label>
                    <input :checked="item[2]" type="checkbox" @change="updateServiceInHandyMan(item[0],$event)" /><span></span>
                  </label>
                </td>
             </tr>
           </tbody>
         </table>
       
         
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "serviceInHandyman",
  components: {},

  data() {
    return {
      handymanId: this.$route.params.handymanId,
      serviceName: null,
      services: [],
      dataset: []
    };
  },

  mounted() {
    

    if (this.handymanId != null) {
      this.loadHandymanInfo(this.handymanId);
      this.listServices(this.handymanId);
    }
  },
  methods: {
    loadHandymanInfo(handymainId) {
      console.log(handymainId);
      // also get the building info
      axios
        .get(
          process.env.ROOT_API +
            "ServiceInHandyman/Get?handymanId=" +
            handymainId,
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
    updateServiceInHandyMan(serviceId,event) {

      var insertOrDelete = "delete";
      if (event.target.checked){
        insertOrDelete = "insert";
      }


      let data = {
        UserId: this.handymanId,
        ServiceId: serviceId,
        InsertOrDelete:insertOrDelete
      };

      axios
        .post(
          process.env.ROOT_API + "ServiceInHandyman/Update",         
          JSON.stringify(data),
          this.$store.getters.getTokenHeader
          )
        .then(response => {
          console.log("updated");
        })
        .catch(error => {
          console.log(error);
        });
    },
    listServices(handymainId) {
      axios
        .get(
          process.env.ROOT_API +
            "ServiceInHandyman/List?handymanId=" +
            handymainId,
            this.$store.getters.getTokenHeaderFormData
        )
        .then(response => {
          //console.log(response.data);
          response.data.forEach(el => { 
            
            var isChecked = false;
            if (el.userId != null) {
              isChecked = true;
            }
            
            this.dataset.push([
              el.serviceId,
              el.serviceName,
              isChecked      
            ]);
          });

          //this.initializeDataTable();
        })
        .catch(error => {
          console.log(error);
        });
    },





    initializeDataTable() {
      $("#services").DataTable({
        data: this.dataset,
        columns: [{ title: "Id" }, { title: "חוק המכר" }, { title: "" }],
        destroy: true,
        fixedColumns: true,
        order: [0, "asc"],
        paging: false
      });

      // style 'show entries' elements
      $(".mdl-cell--6-col:first").attr("style", "display:none");
      // style 'the search' elements
      /*$(".mdl-cell--6-col").attr(
        "class",
        "mdl-cell--12-col-phone mdl-cell--10-col"
      ); */
    }
  }
};
</script>

<style>
th:nth-child(1) {
  width: 75px !important;
}
th:nth-child(n + 6) {
  width: 75px !important;
}
th {
  width: 50%;
  background: #f7f7f7;
}
.material-table th {
  text-align: center !important;
}
.material-table td {
  text-align: center !important;
}
.mdl-grid {
  width: 100%;
}

.material-table select {
  display: block !important;
}

div.material-table {
  padding: 0;
}

div.material-table .hiddensearch {
  padding: 0 14px 0 24px;
  border-bottom: solid 1px #dddddd;
  display: none;
}

div.material-table .hiddensearch input {
  margin: 0;
  border: transparent 0 !important;
  height: 48px;
  color: rgba(0, 0, 0, 0.84);
}

div.material-table .hiddensearch input:active {
  border: transparent 0 !important;
}

div.material-table table {
  table-layout: fixed;
}

div.material-table .table-header {
  height: 64px;
  padding-left: 24px;
  padding-right: 14px;
  -webkit-align-items: center;
  -ms-flex-align: center;
  align-items: center;
  display: flex;
  -webkit-display: flex;
  border-bottom: solid 1px #dddddd;
}

div.material-table .table-header .actions {
  display: -webkit-flex;
  margin-left: auto;
}

div.material-table .table-header .btn-flat {
  min-width: 36px;
  padding: 0 8px;
}

div.material-table .table-header input {
  margin: 0;
  height: auto;
}

div.material-table .table-header i {
  color: rgba(0, 0, 0, 0.54);
  font-size: 24px;
}

div.material-table .table-footer {
  height: 56px;
  padding-left: 24px;
  padding-right: 14px;
  display: -webkit-flex;
  display: flex;
  -webkit-flex-direction: row;
  flex-direction: row;
  -webkit-justify-content: flex-end;
  justify-content: flex-end;
  -webkit-align-items: center;
  align-items: center;
  font-size: 12px !important;
  color: rgba(0, 0, 0, 0.54);
}

div.material-table .table-footer .dataTables_length {
  display: -webkit-flex;
  display: flex;
}

div.material-table .table-footer label {
  font-size: 12px;
  color: rgba(0, 0, 0, 0.54);
  display: -webkit-flex;
  display: flex;
  -webkit-flex-direction: row;
  /* works with row or column */

  flex-direction: row;
  -webkit-align-items: center;
  align-items: center;
  -webkit-justify-content: center;
  justify-content: center;
}

div.material-table .table-footer .select-wrapper {
  display: -webkit-flex;
  display: flex;
  -webkit-flex-direction: row;
  /* works with row or column */

  flex-direction: row;
  -webkit-align-items: center;
  align-items: center;
  -webkit-justify-content: center;
  justify-content: center;
}

div.material-table .table-footer .dataTables_info,
div.material-table .table-footer .dataTables_length {
  margin-right: 32px;
}

div.material-table .table-footer .material-pagination {
  display: flex;
  -webkit-display: flex;
  margin: 0;
}

div.material-table .table-footer .material-pagination li:first-child {
  margin-right: 24px;
}

div.material-table .table-footer .material-pagination li a {
  color: rgba(0, 0, 0, 0.54);
}

div.material-table .table-footer .select-wrapper input.select-dropdown {
  margin: 0;
  border-bottom: none;
  height: auto;
  line-height: normal;
  font-size: 12px;
  width: 40px;
  text-align: right;
}

div.material-table .table-footer select {
  background-color: transparent;
  width: auto;
  padding: 0;
  border: 0;
  border-radius: 0;
  height: auto;
  margin-left: 20px;
}

div.material-table .table-title {
  font-size: 20px;
  color: #000;
}

div.material-table table tr td {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

div.material-table table tr td a {
  color: inherit;
}

div.material-table table tr td a i {
  font-size: 18px;
  color: rgba(0, 0, 0, 0.54);
}

div.material-table table tr {
  font-size: 12px;
}

div.material-table table th {
  color: #757575;
  cursor: pointer;
  white-space: nowrap;
  padding: 0;
  height: 56px;
  vertical-align: middle;
  outline: none !important;
}

div.material-table table th.sorting_asc,
div.material-table table th.sorting_desc {
  color: rgba(0, 0, 0, 0.87);
}

div.material-table table th.sorting:after,
div.material-table table th.sorting_asc:after,
div.material-table table th.sorting_desc:after {
  font-family: "Material Icons";
  font-weight: normal;
  font-style: normal;
  font-size: 16px;
  line-height: 1;
  letter-spacing: normal;
  text-transform: none;
  display: inline-block;
  word-wrap: normal;
  -webkit-font-feature-settings: "liga";
  -webkit-font-smoothing: antialiased;
  content: "arrow_back";
  -webkit-transform: rotate(90deg);
  display: none;
  vertical-align: middle;
}

div.material-table table th.sorting:hover:after,
div.material-table table th.sorting_asc:after,
div.material-table table th.sorting_desc:after {
  display: inline-block;
}

div.material-table table th.sorting_desc:after {
  content: "arrow_forward";
}

div.material-table table tbody tr:hover {
  background-color: rgb(238, 238, 238);
}

div.material-table table th:first-child,
div.material-table table td:first-child {
  padding: 0 0 0 24px;
}

div.material-table table th:last-child,
div.material-table table td:last-child {
  padding: 0 14px 0 0;
}

div.dt-button-info {
  position: fixed;
  top: 50%;
  left: 50%;
  width: 400px;
  margin-top: -100px;
  margin-left: -200px;
  text-align: center;
  z-index: 21;
  color: rgba(0, 0, 0, 0.6);
}

@media screen and (max-width: 640px) {
  div.dt-buttons {
    float: none !important;
    text-align: center;
  }
}
</style>
