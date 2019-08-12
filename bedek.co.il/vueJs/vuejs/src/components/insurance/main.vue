<template>
    <div class="container">
        skjkj
    </div>
</template>

<script>
import axios from "axios";
import moment from "moment";

export default {
 name: "handyman",
  data() {
    return {
     
    };
  },
   mounted() {
    this.$store.commit("setInfoBarText", "הוסף שירות תחת ביטוח");
    
  },
}
</script>

<style>

</style>
