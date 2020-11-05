<template>
  <el-option
    :value="value"
    :label="text"
    :disabled="readonly"
  />
</template>
<script>
export default {
  name: 'TkComboItem',
  props: {
    value: {
      type: [String, Number]
    },
    text: {
      type: String
    },
    readonly: {
      type: Boolean
    },
    addition: {
      type: Object
    }
  }
}
</script>