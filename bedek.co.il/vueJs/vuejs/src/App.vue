<template>
  <div id="app">
    <Navbar/>
    <InfoBar />
    <router-view/>
  </div>
</template>

<script>

import Navbar from "@/components/layout/navbar";
import InfoBar from "@/components/layout/infobar";


export default {
  name: "App",
  components: {
    Navbar,
    InfoBar
  }
};
</script>

<style>
 @import './assets/style/main.css';
 

</style>
