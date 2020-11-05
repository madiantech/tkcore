<script>
  export default {
    name: 'MenuItem',
    functional: true,
    props: {
      icon: {
        type: String,
        default: ''
      },
      title: {
        type: String,
        default: ''
      }
    },
    /* render(h, context) {
      const { icon, title } = context.props
      const vnodes = []

      if (icon) {
        vnodes.push(<svg-icon icon-class={icon}/>)
      }

      if (title) {
        vnodes.push(<span slot='title'>{(title)}</span>)
      }
      return vnodes
    }*/
    render: function(createElement, context) {
      const vnodes = []
      const { icon, title } = context.props
      if (icon) {
        const elHtml = createElement('svg-icon', {
          attrs: {
            'icon-class': icon
          }
        })

        vnodes.push(elHtml)
      }

      if (title) {
        const elHtml = createElement('span', {
          attrs: {
            slot: title,
            class: 'system-menu-title'
          }
        }, title)
        vnodes.push(elHtml)
      }
      return vnodes
    }
  }
</script>
