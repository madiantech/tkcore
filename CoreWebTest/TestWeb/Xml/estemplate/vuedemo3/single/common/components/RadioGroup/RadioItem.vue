<template>
  <el-radio :label="value">{{text||value}}</el-radio>
</template>
<script>
export default {
  name: 'TkRadioItem',
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