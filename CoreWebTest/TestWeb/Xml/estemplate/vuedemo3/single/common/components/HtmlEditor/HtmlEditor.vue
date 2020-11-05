
<template>
  <editor
    v-model="input"
    :init="init"
  />
</template>
<script>
import tinymce from 'tinymce/tinymce'
import Editor from '@tinymce/tinymce-vue'
import 'tinymce/themes/silver'
import 'tinymce/plugins/autolink'
import 'tinymce/plugins/link'
import 'tinymce/plugins/autosave'
import 'tinymce/plugins/paste'
import 'tinymce/plugins/help'
export default {
  name: 'TkHtmlEditor',
  components: {
    Editor
  },
  props: {
    value: {
      type: String
    }
  },
  data () {
    return {
      init: {
        language_url: '/static/tinymce/langs/zh_CN.js',// 语言包的路径
        language: 'zh_CN',//语言
        skin_url: '/static/tinymce/skins/ui/oxide',// skin路径
        content_css: '/static/tinymce/skins/content/default/content.css',
        icons_url: '/static/tinymce/icons/default/icons.js',
        branding: false,//是否禁用“Powered by TinyMCE”
        plugins: 'autolink link autosave paste help',
        toolbar: 'undo redo | styleselect | fontsizeselect bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | outdent indent | link paste help',
      }
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
  }
  // mounted () { 
  //   tinymce.init({})
  // }
}
</script>