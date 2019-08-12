<template>
  <div class="icon" :class="[`icon--${name}`, `icon--${size}`, { 'has-align-fix': fixAlign }]">
    <svg class="icon__svg">
      <use xmlns:xlink="http://www.w3.org/1999/xlink" :xlink:href="'#icon--' + name"></use>
    </svg>
  </div>
</template>

<script>
export default {
  props: {
    name: {},
    size: {
      default: 'normal',
    },
    modifier: {
      default: null,
    },
    fixAlign: {
      default: true,
    },
  },
}
</script>
  