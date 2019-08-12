<template>
  <div class="container" v-if="this.infoString">
    <h3>{{ infoString }}</h3>
  </div>
</template>

<script>
export default {
  name: "infoBar",
  props: ["propInfoBar"],
  data() {
    return {};
  },
  computed: {
    infoString: function() {
      return this.$store.state.infoBarText;
    }
  }
};
</script>

