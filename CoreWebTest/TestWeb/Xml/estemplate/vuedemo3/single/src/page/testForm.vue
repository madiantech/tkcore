<template>
    <el-form :model="form"
             label-width="80px"
             inline>
        <el-row>
            <el-col :span="8">
                <tk-input name="input"
                          caption="input"
                          size="small"
                          tooltip="input"
                          v-model="form.input">
                    <template #front>
                        Http://
                    </template>
                </tk-input>
            </el-col>
            <el-col :span="8">
                <tk-textarea name="textarea"
                             caption="textarea"
                             v-model="form.textarea"
                             :rows="8" />
            </el-col>
            <el-col :span="8">
                <tk-switch name="switch"
                           caption="switch"
                           v-model="form.switch" />
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-checkbox name="checkbox"
                             caption="checkbox"
                             v-model="form.checkbox" />
            </el-col>
            <el-col :span="8">
                <tk-radio-group name="radioItem"
                                caption="RadioItem"
                                v-model="form.radioItem">
                    <tk-radio-item :value="0"
                                   text="零" />
                    <tk-radio-item :value="1"
                                   text="一" />
                </tk-radio-group>
            </el-col>
            <el-col :span="8">
                <tk-radio-group name="radioButtonItem"
                                caption="RadioButtonItem"
                                v-model="form.radioButtonItem">
                    <tk-radio-button-item :value="0"
                                          text="零" />
                    <tk-radio-button-item :value="1"
                                          text="一" />
                </tk-radio-group>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-check-list name="checkList"
                               caption="CheckList"
                               v-model="form.checkList">
                    <tk-check-item value="0"
                                   text="零" />
                    <tk-check-item value="1"
                                   text="一" />
                    <tk-check-item value="2"
                                   text="二" />
                </tk-check-list>
            </el-col>
            <el-col :span="8">
                <tk-select name="select"
                           caption="select"
                           v-model="form.select">
                    <tk-combo-item :value="0"
                                   text="零" />
                    <tk-combo-item :value="1"
                                   text="一" />
                    <tk-combo-item :value="2"
                                   text="二" />
                </tk-select>
            </el-col>
            <el-col :span="8">
                <tk-easysearch name="easysearch"
                               caption="easysearch"
                               size="small"
                               v-model="form.easysearch" />
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-easysearch-text name="easysearchText"
                                    caption="easysearchText"
                                    v-model="form.easysearchText" />
            </el-col>
            <el-col :span="8">
                <tk-multiple-easysearch name="multipleEasysearch"
                                        caption="multipleEasysearch"
                                        v-model="form.multipleEasysearch" />
            </el-col>
            <el-col :span="8">
                <tk-label name="label"
                          caption="label"
                          :value="form.label" />
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-time name="time"
                         caption="time"
                         v-model="form.time"
                         tooltip="time"
                         selectable-range="18:30:00 - 20:30:00" />
            </el-col>
            <el-col :span="8">
                <tk-date name="date"
                         caption="date"
                         v-model="form.date"
                         tooltip="date" />
            </el-col>
            <el-col :span="8">
                <tk-datetime name="datetime"
                             caption="datetime"
                             v-model="form.datetime"
                             tooltip="datetime" />
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-password name="password"
                             caption="password"
                             v-model="form.password"
                             showicon />
            </el-col>
            <el-col :span="8">
                <tk-number name="number"
                           caption="number"
                           v-model="form.number" />
            </el-col>
            <el-col :span="8">
                <tk-upload name="upload"
                           caption="upload">
                    <el-button size="small"
                               type="primary">点击上传</el-button>
                    <template #tip>
                        <div class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
                    </template>
                </tk-upload>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-input-range name="inputRange"
                                caption="inputRange"
                                size="small"
                                v-model="form.inputRangeStart"
                                :end-value.sync="form.inputRangeEnd" />
            </el-col>
            <el-col :span="8">
                <tk-date-range name="dateRange"
                               caption="dateRange"
                               size="small"
                               v-model="form.dateRangeStart"
                               :end-value.sync="form.dateRangeEnd" />
            </el-col>
            <el-col :span="8">
                <tk-datetime-range name="datetimeRange"
                                   caption="datetimeRange"
                                   v-model="form.datetimeRangeStart"
                                   :end-value.sync="form.datetimeRangeEnd" />
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="8">
                <tk-number-range name="numberRange"
                                 caption="numberRange"
                                 size="small"
                                 v-model="form.numberRangeStart"
                                 :end-value.sync="form.numberRangeEnd" />
            </el-col>
            <el-col :span="16">
                <tk-html-editor />
            </el-col>
        </el-row>
    </el-form>
</template>
<script>
    export default {
        name: 'TestForm',
        data() {
            return {
                form: {
                    input: '',
                    textarea: '',
                    switch: '0',
                    checkbox: '0',
                    radioItem: -1,
                    radioButtonItem: -1,
                    checkList: '0,1',
                    select: 0,
                    easysearch: '',
                    easysearchText: '',
                    multipleEasysearch: {},
                    label: 'test label',
                    time: '',
                    date: '',
                    datetime: '',
                    password: '',
                    number: 0,
                    inputRangeStart: '',
                    inputRangeEnd: '',
                    dateRangeStart: '',
                    dateRangeEnd: '',
                    datetimeRangeStart: '',
                    datetimeRangeEnd: '',
                    numberRangeStart: 0,
                    numberRangeEnd: 1
                }
            }
        }
    }
</script>