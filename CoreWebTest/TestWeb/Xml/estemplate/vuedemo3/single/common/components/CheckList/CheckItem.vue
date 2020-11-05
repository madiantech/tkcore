<template>
  <el-checkbox :label="value">{{text||value}}</el-checkbox>
</template>
<script>
export default {
  name: 'TkCheckItem',
  props: {
    value: {
      type: [String, Number]
    },
    text: {
      type: String
    }
  }
}
</script>