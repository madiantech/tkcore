<template>
  <el-form-item
    :prop="name"
    :class="classname"
    :label="caption"
    :label-width="labelWidth"
    :required="required"
    :size="size"
    v-tk-help="tooltip"
  >
    <el-upload
      :action="uploadUrl"
      :before-upload="beforeUpload"
      :accept="fileType"
      :on-success="onSuccess"
      :on-preview="onPreview"
      :multiple="multiple"
      :limit="limit"
    >
      <slot />
      <template
        v-if="$slots.tip"
        slot="tip"
      >
        <slot name="tip" />
      </template>
    </el-upload>
    <el-image-viewer
      v-show="imagePreview"
      :url-list="[image]"
      :on-close="()=>{imagePreview=false}"
    />
  </el-form-item>
</template>
<script>
import ImageViewer from 'element-ui/packages/image/src/image-viewer'
export default {
  name: 'TkUpload',
  components: {
    [ImageViewer.name]: ImageViewer
  },
  props: {
    notform: {
      type: Boolean,
      default: false
    },
    name: {
      type: String,
      required: true
    },
    caption: {
      type: String
    },
    classname: {
      type: String
    },
    value: {
      type: [String],
      default: ''
    },
    readonly: {
      type: Boolean
    },
    uploadUrl: {
      type: String,
      default: ''
    },
    maxSize: {
      type: Number
    },
    fileType: {
      type: String
    },
    fileSize: {
      type: String
    },
    serverPath: {
      type: String
    },
    contentType: {
      type: String
    },
    afterUpload: {
      type: Function
    },
    preview: {
      type: Boolean
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
    },
    size: {
      type: String
    },
    labelWidth: {
      type: String
    },
    multiple: {
      type: Boolean
    },
    limit: {
      type: Number
    }
  },
  data () {
    return {
      imagePreview: false,
      image: ''
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
  methods: {
    beforeUpload (file) {
      if (this.maxSize && file.size < this.maxSize) {
        return false
      }
      return true
    },
    onSuccess (response, file, fileList) {
      if (this.afterUpload) this.afterUpload(response, file, fileList)
    },
    onPreview (file) {
      if (this.contentType.indexOf('image') > -1 && this.preview) {//预览
        this.image = file.src
        this.imagePreview = true
      }
    }
  }
}
</script>