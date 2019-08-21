<template>
  <div>
    <div class="container title-container hand" @click="toggleExpande('add')">
      <span class="title">הוסף חוק מכר</span>
      <i class="material-icons" v-if="expandMoreAdd">expand_more</i>
      <i class="material-icons" v-if="!expandMoreAdd">expand_less</i>
    </div>
    <div v-show="expandMoreAdd">
      <addUpdateService
        v-on:loadApartmentList="refreshListFromEmit()"
        :propIsFromListServices="true"
      />
    </div>
    <div class="container">
      <!-- list-services -->
      <div class="list-handyman">
        <div class="title-container hand" @click="toggleExpande('list')">
          <span class="title">רשימת חוקי המכר</span>
          <i class="material-icons" v-if="expandMoreList">expand_more</i>
          <i class="material-icons" v-if="!expandMoreList">expand_less</i>
        </div>
        <div v-show="expandMoreList">
          <div id="vgt">
            <vue-good-table
              :columns="columns"
              :rows="rows"
              :rtl="true"
              :search-options="{ enabled: true,placeholder: ' חפש בטבלה ',}"
              :pagination-options="{ enabled: true, perPage: 50 , perPageDropdown: [100]}"
              styleClass="vgt-table"
            >
              <div slot="emptystate">אין נתונים בטבלה</div>
            </vue-good-table>
          </div>
        </div>
      </div>
      <!-- /list-services -->
    </div>
  </div>
</template>




<script>
import axios from "axios";
import addUpdateService from "@/components/service/add-update";
import "vue-good-table/dist/vue-good-table.css";
import { VueGoodTable } from "vue-good-table";

export default {
  components: {
    name: "serviceList",
    props: ["propIsFromListServices"],
    addUpdateService,
    VueGoodTable
  },

  data() {
    return {
      expandMoreAdd: false,
      expandMoreList: true,
      feedback: null,
      street: null,
      serviceName: null,
      
      rows: [],
      columns: [
        {
          label: "Id",
          field: "serviceId",
          type: "number"
        },
        {
          label: "חוק המכר",
          field: "serviceName"
        },
        {
          label: "שנות אחריות",
          field: "warrantyPeriodInMonths",
          type: "number"
        },
        {
          label: "עדכון",
          field: "update",
          html: true
        }
      ]
    };
  },
  mounted() {
    this.$store.commit("setInfoBarText", "חוקי מכר");
    this.listServices();
  },
  methods: {
    refreshListFromEmit() {
      this.listServices();
    },
    toggleExpande(area) {
      if (area == "add") {
        this.expandMoreAdd = !this.expandMoreAdd;
      }
      if (area == "list") {
        this.expandMoreList = !this.expandMoreList;
      }
    },

    listServices() {
      

      axios
        .get(
          process.env.ROOT_API + "Service/List",
          this.$store.getters.getTokenHeaderFormData
        )
        .then(res => {
          res.data.forEach(el => {
            var updateLink = this.$router.resolve({
              name: "addUpdateService",
              params: { serviceId: el.serviceId }
            }).href;

            el.update = `<a href='${updateLink}'><i class="material-icons">edit</i></a>`;
            el.warrantyPeriodInMonths = el.warrantyPeriodInMonths / 12; // show in year,
          });

          this.rows = res.data;
        })
        .catch(error => {
          console.log(error);
        });
    }
  }
};
</script>

  