 <template> 
  <div id="app">
    <router-view />
  </div>
</template>
<script>
export default {
  created () {
    console.log('start')
  }
}
</script>
<style lang="scss">
html {
  height: 100%;
}
body {
  height: calc(100% - 16px);
  margin: 8px;
}
#app {
  // margin: 8px;
  // height: calc(100% - 16px);
  height: 100%;
}
</style>