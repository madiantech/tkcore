<template>
  <el-radio-button :label="value">{{text||value}}</el-radio-button>
</template>
<script>
export default {
  name: 'TkRadioButtonItem',
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