<template>
  <el-form-item
    :prop="name"
    :class="classname"
    :label="caption"
    :label-width="labelWidth"
    :size="size"
  >
    {{value}}
  </el-form-item>
</template>
<script>
export default {
  name: 'TkLabel',
  props: {
    name: {
      type: String,
      required: true
    },
    caption: {
      type: String
    },
    labelWidth: {
      type: String
    },
    classname: {
      type: String
    },
    value: {
      type: [String, Number],
      default: ''
    },
    size: {
      type: String
    }
  }
}
</script>