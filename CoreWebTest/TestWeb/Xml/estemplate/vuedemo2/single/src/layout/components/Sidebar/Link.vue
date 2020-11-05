
<template>
  <!-- eslint-disable vue/require-component-is -->
  <component v-bind="linkProps(to)">
    <slot />
  </component>
</template>

<script>

  export default {
    props: {
      to: {
        type: String,
        required: true
      }
    },
    methods: {
      linkProps(url) {
        if (this.isExternal(url)) {
          return {
            is: 'a',
            href: url,
            target: '_blank',
            rel: 'noopener'
          }
        }
        return {
          is: 'router-link',
          to: url
        }
      },
      isExternal(path) {
        return /^(https?:|mailto:|tel:)/.test(path)
      }
    }
  }
</script>
