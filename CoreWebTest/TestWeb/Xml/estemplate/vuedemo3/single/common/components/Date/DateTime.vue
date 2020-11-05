<template>
  <tk-date
    type="datetime"
    v-model="input"
    :placeholder="placeholder"
    :format="format"
    v-bind="$attrs"
  />
</template>
<script>
import TkDate from './Date'
export default {
  name: 'TkDatetime',
  components: {
    TkDate
  },
  props: {
    value: {
      type: [String, Date],
      default: ''
    },
    placeholder: {
      type: String,
      default: '选择日期时间...'
    },
    format: {
      type: String,
      default: 'yyyy-MM-dd HH:mm'
    }
  },
  computed: {
    input: {
      get () {
        return this.value
      },
      set (val) {
        this.$emit('input', val)
      }
    }
  },
}
</script>