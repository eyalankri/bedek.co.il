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
            :row-style-class="rowStyleClassFn">
            >
          </vue-good-table>
<!-- click on a row below to show the action button -->
  </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";
import 'vue-good-table/dist/vue-good-table.css'
import { VueGoodTable } from 'vue-good-table';

export default {
 name: "handymanInBuilding",
 components:{VueGoodTable},
 data(){
     return{
        buildingId : this.$route.params.buildingId,
        street: null,
        buildingNumber: null,
        projectName: null,
        city: null,
        dataset: [],

 columns: [
        {
          label: 'Name',
          field: 'name',
        },
        {
          label: 'Age',
          field: 'age',
          type: 'number',
        },
        {
          label: 'Created On',
          field: 'createdAt',
          type: 'date',
          dateInputFormat: 'yyyy-MM-dd',
          dateOutputFormat: 'MMM Do yy',
        },
        {
          label: 'Percent',
          field: 'score',
          type: 'percentage',
        },
      ],
      rows: [
        { id:1, name:"John", age: 20, createdAt: '2011-10-31',score: 0.03343 },
        { id:2, name:"Jane", age: 24, createdAt: '2011-10-31', score: 0.03343 },
        { id:3, name:"Susan", age: 16, createdAt: '2011-10-30', score: 0.03343 },
        { id:4, name:"Chris", age: 55, createdAt: '2011-10-11', score: 0.03343 },
        { id:5, name:"Dan", age: 40, createdAt: '2011-10-21', score: 0.03343 },
        { id:6, name:"John", age: 20, createdAt: '2011-10-31', score: 0.03343 },
      ],
        
     }
 },
  mounted() {     
      this.loadBuilding();
      this.loadServiceInHandymanInBuilding();

      
      this.rows.forEach(function(row){
        if (row.name=='Dan') {
          console.log(123123);
          $('.clsRow_' + row.name).find('th.vgt-checkbox-col').click();  
        }
        
      });
  }, 
  methods: {
    rowStyleClassFn(row){
       return row.name == 'Dan' ? 'clsRow_' + row.name : '';
    },
    selectionChanged(params){
      var arr = params.selectedRows;
      arr.forEach(function(element) {
      console.log(element.name, element.age);
});
      
       
    },
    loadBuilding() {             

      axios
        .get(
          process.env.ROOT_API + "Building/Get?buildingId=" +
            this.buildingId,
            this.$store.getters.getTokenHeaderFormData
        )
        .then(res => {
          
          this.projectName = res.data.projectName;
          this.city = res.data.city;
          this.street = res.data.street;
          this.buildingNumber = res.data.buildingNumber;
          this.$store.commit("setInfoBarText",`שיוך אנשי מקצוע: ${this.projectName} ${this.buildingNumber} - ${this.city}`);
           
        })
        .catch(error => {
          console.log(error);
        });

     
    },
    loadServiceInHandymanInBuilding(){
        this.dataset=[];
         axios
        .get(
          process.env.ROOT_API + "ServiceInHandymanInBuilding/List?buildingId=" +
            this.buildingId,
            this.$store.getters.getTokenHeaderFormData
        )
         .then(response => {
          response.data.forEach(el => {
            this.dataset.push([
              //el.UserId,
              el.serviceName,
              el.company,
              el.firstName,
              el.lastName,
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
        data: this.dataset,
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
          { title: "" },                         
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
    },
   
   
}
  
}
$(function(){

$('.abc').change(function(){
  alert(123)
  })
})


</script>

<style>

</style>
