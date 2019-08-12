<template>
  <div class="container">
    <!-- add-building -->
    <div class="add-building">
      <div class="title-container hand" @click="toggleExpande('add')">
        <span class="title">הוסף בניין</span>
        <i class="material-icons" v-if="expandMoreAdd">expand_more</i>
        <i class="material-icons" v-if="!expandMoreAdd">expand_less</i>
      </div>
      <div v-show="expandMoreAdd">
        <form class="col s12">
          <div class="row">
            <div class="input-field col s12 m6">
              <input id="projectName" name="projectName" type="text" v-model="projectName" />
              <label for="projectName">שם הפרוייקט</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="city" name="city" type="text" v-model="city" />
              <label for="city">* עיר</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="street" name="street" type="text" v-model="street" />
              <label for="street">* רחוב</label>
            </div>
            <div class="input-field col s12 m6">
              <input id="number" name="buildingNumber" type="text" v-model="buildingNumber" />
              <label for="buildingNumber">* מספר בניין</label>
            </div>
          </div>

          <div class="row">
            <p class="red-text right" v-if="feedback">{{ feedback }}</p>
            <div class="input-field col s12">
              <a @click="addBuilding" class="waves-effect waves-light btn right">הוסף בניין</a>
            </div>
          </div>
        </form>
      </div>
    </div>
    <!-- /add-building -->
    <!-- list-buildings -->
    <div class="list-buildings">
      <div class="title-container hand" @click="toggleExpande('list')">
        <span class="title">בניינים קיימים</span>
        <i class="material-icons" v-if="expandMoreList">expand_more</i>
        <i class="material-icons" v-if="!expandMoreList">expand_less</i>
      </div>
      <div v-show="expandMoreList" class="material-table">
        <table id="buildings" class="mdl-data-table" width="100%"></table>
      </div>
    </div>
    <!-- /list-buildings -->
  </div>
</template>




<script>
import axios from "axios";

export default {
  name: "buildings",

  data() {
    return {
      expandMoreAdd: false,
      expandMoreList: true,
      feedback: null,
      street: null,
      buildingNumber: null,
      projectName: null,
      city: null,
      buildingId: null,
      newBuilding: [],
      buildings: [],
      dataset: []
    };
  },
  mounted() {
    this.$store.commit("setInfoBarText", "בניינים");
    this.listBuildings();
  },
  methods: {
    toggleExpande(area) {
      if (area == "add") {
        this.expandMoreAdd = !this.expandMoreAdd;
      }
      if (area == "list") {
        this.expandMoreList = !this.expandMoreList;
      }
    },
    addBuilding() {
      this.feedback = "";

      if (!this.city) {
        this.feedback = "יש לרשום עיר.";
        return false;
      }
      if (!this.street) {
        this.feedback = "יש לרשום כתובת.";
        return false;
      }
      if (!this.buildingNumber) {
        this.feedback = "יש לרשום את מספר הבניין.";
        return false;
      }

      let data = {
        City: this.city,
        Street: this.street,
        BuildingNumber: this.buildingNumber,
        ProjectName: this.projectName
      };

      axios
        .post(
          process.env.ROOT_API + "Building/Add",
          data,
          this.$store.getters.getTokenHeader
        )
        .then(res => {
                               
          this.city = null;
          this.street = null;
          this.buildingNumber = null;
          this.projectName = null;

          var table = $("#buildings").DataTable();
          table.destroy(); // must destroy before calling again
          this.listBuildings();
           
        })
        .catch(error => {
          this.$router.push({
            name: "Login"
          });
        });
    },
    listBuildings() {
      axios
        .get(
          process.env.ROOT_API + "Building/List",
          this.$store.getters.getTokenHeaderFormData
        )
        .then(response => {
          response.data.forEach(el => {
            
            var updateBuilding = this.$router.resolve({
              name: "updateBuilding",
              params: { buildingId: el.buildingId }
            }).href;

            var listApartment = this.$router.resolve({
              name: "listApartment",
              params: { id: el.buildingId }
            }).href;

            var handymanInBuilding = this.$router.resolve({
              name: "handymanInBuilding",
              params: { buildingId: el.buildingId }
            }).href;

            this.dataset.push([
              el.buildingId,
              el.projectName,
              el.city,
              el.street,
              el.buildingNumber,
              `<a href='${updateBuilding}'><i class="material-icons">edit</i></a>`,
              `<a href='${handymanInBuilding}'><i class="material-icons">build</i></a>`,
              `<a href='${listApartment}'><i class="material-icons">home</i></a>`
            ]);
          });

          this.initializeDataTable();
        })
        .catch(error => {
          console.log(error);
        });
    },
    initializeDataTable() {
      $("#buildings").DataTable({
        data: this.dataset,
        columns: [
          { title: "Id" },
          { title: "פרוייקט" },
          { title: "עיר" },
          { title: "רחוב" },
          { title: "מספר" },
          { title: "עדכן" },
          { title: "אנשי מקצוע" },
          { title: "דירות" }
        ],
        fixedColumns: true,
        order: [0, "desc"],
        language: {
          processing: "מעבד...",
          lengthMenu: "הצג _MENU_ פריטים",
          zeroRecords: "לא נמצאו רשומות מתאימות",
          emptyTable: "לא נמצאו רשומות מתאימות",
          info: "_START_ עד _END_ מתוך _TOTAL_ רשומות",
          infoEmpty: "0 עד 0 מתוך 0 רשומות",
          infoFiltered: "(מסונן מסך _MAX_  רשומות)",
          infoPostFix: "",
          search: "חפש:",
          url: "",
          paginate: {
            first: "ראשון",
            previous: "הקודם",
            next: "הבא",
            last: "אחרון"
          }
        }
      });

      // style the search & 'show entries' elements
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
</script>

 

<style>
th:nth-child(1) {
  width: 75px !important;
}
th:nth-child(n + 6) {
  width: 75px !important;
}
th {
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
  padding: 0;
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
