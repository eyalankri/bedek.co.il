<template>
  <div class="showApartment">
    <div>
      <div class="container">
        <div class="title-container hand" @click="toggleExpande('details')">
          <span class="title">פרטי הדירה והדייר</span>
          <i class="material-icons" v-if="expandMoreDetails">expand_more</i>
          <i class="material-icons" v-if="!expandMoreDetails">expand_less</i>
        </div>
      </div>
      <div v-show="expandMoreDetails">
        <addUpdateApartments :propApartmentId="this.apartmentId" @sendBuildingId="getBuildingIdFromChild"/>
      </div>
    </div>
    <div>
      <div class="container">
        <div class="title-container hand" @click="toggleExpande('docs')">
          <span class="title">מסמכים</span>
          <i class="material-icons" v-if="expandMoreDocs">expand_more</i>
          <i class="material-icons" v-if="!expandMoreDocs">expand_less</i>
        </div>
      </div>
      <div v-show="expandMoreDocs">
        <docs :propApartmentId="this.apartmentId"/>
      </div>
    </div>
     <div class="container">
      <a @click="backToApartmentList" class="waves-effect waves-light btn right">חזרה לרשימת הדירות</a>
    </div>
  </div>
</template>

<script>
import addUpdateApartments from "@/components/apartment/add-update";
import docs from "@/components/apartment/docs";

export default {
  name: "showApartment",
  components: {
    addUpdateApartments,
    docs
  },
  data() {
    return {
      expandMoreDetails: false,
      expandMoreDocs: false,
      apartmentId: this.$route.params.id, 
      buildingId: null
    };
  },
  methods: {
    toggleExpande(area) {
      if (area == "details") {
        this.expandMoreDetails = !this.expandMoreDetails;
      }
      if (area == "docs") {
        this.expandMoreDocs = !this.expandMoreDocs;
      }
    },    
    backToApartmentList() {
      this.$router.push({
        name: "listApartment",
        params: { id: this.buildingId }
      });
    },
    getBuildingIdFromChild(id){
      this.buildingId = id;
    },
  }
};
</script>

<style>
</style>
