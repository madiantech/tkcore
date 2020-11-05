<template>
  <tk-date-range
    type="datetimerange"
    v-model="input"
    :end-value.sync="endInput"
    :placeholder="placeholder"
    :format="format"
    v-bind="$attrs"
  />
</template>
<script>
import TkDateRange from './DateRange'
export default {
  name: 'TkDatetimeRange',
  components: {
    TkDateRange
  },
  props: {
    value: {
      type: [String, Date],
      default: ''
    },
    endValue: {
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
    },
    endInput: {
      get () {
        return this.endValue
      },
      set (val) {
        this.$emit('update:endValue', val)
      }
    }
  },
}
</script>