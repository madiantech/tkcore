<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:tk="http://www.qdocuments.net" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <tk:Toolkit version="5.0" xsi:schemaLocation="http://www.qdocuments.net ..\..\schema\v5\DataXml.xsd" xmlns:tk="http://www.qdocuments.net" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
      <xsl:apply-templates select="/Toolkit/tk:Table"/>
    </tk:Toolkit>
  </xsl:template>
  <xsl:template match="tk:Table">
    <tk:Table TableName="{@TableName}">
      <tk:TableDesc>
        <tk:Content>
          <xsl:value-of select="@TableDesc"/>
        </tk:Content>
      </tk:TableDesc>
      <xsl:apply-templates select="tk:Field"/>
    </tk:Table>
  </xsl:template>
  <xsl:template match="tk:Field">
    <tk:Field DataType="{@DataType}">
      <xsl:if test="@IsKey">
        <xsl:attribute name="IsKey"><xsl:value-of select="@IsKey"/></xsl:attribute>
      </xsl:if>
      <xsl:if test="@IsEmpty">
        <xsl:attribute name="IsEmpty"><xsl:value-of select="@IsEmpty"/></xsl:attribute>
      </xsl:if>
      <xsl:if test="@Kind">
        <xsl:attribute name="Kind"><xsl:value-of select="@Kind"/></xsl:attribute>
      </xsl:if>
      <xsl:if test="@IsAutoInc">
        <xsl:attribute name="IsAutoInc"><xsl:value-of select="@IsAutoInc"/></xsl:attribute>
      </xsl:if>
      <xsl:copy-of select="tk:FieldName"/>
      <tk:DisplayName>
        <tk:Content>
          <xsl:value-of select="tk:DisplayName"/>
        </tk:Content>
      </tk:DisplayName>
      <xsl:if test="tk:Hint">
        <tk:Hint>
          <tk:Content>
            <xsl:value-of select="tk:Hint"/>
          </tk:Content>
        </tk:Hint>
      </xsl:if>
      <xsl:if test="tk:Length">
        <xsl:copy-of select="tk:Length"/>
      </xsl:if>
      <xsl:if test="tk:CodeTable">
        <tk:CodeTable RegName="{tk:CodeTable}"/>
      </xsl:if>
      <xsl:if test="tk:EasySearch">
        <tk:EasySearch RegName="{tk:EasySearch/@RegName}"/>
      </xsl:if>
      <tk:Control Control="{tk:HtmlCtrl/@HtmlCtrl}" Order="{tk:HtmlCtrl/@Order}">
        <xsl:if test="tk:Display/@DefaultShow">
          <xsl:attribute name="DefaultShow"><xsl:value-of select="tk:Display/@DefaultShow"/></xsl:attribute>
        </xsl:if>
      </tk:Control>
      <xsl:if test="tk:Extension/@Search='title'">
        <tk:ListDetail Search="true"/>
      </xsl:if>
      <xsl:if test="tk:Updating or tk:DefaultValue">
        <tk:Edit>
          <xsl:copy-of select="tk:DefaultValue"/>
          <xsl:copy-of select="tk:Updating"/>
        </tk:Edit>
      </xsl:if>
      <xsl:if test="tk:Extension/@CheckValue">
        <tk:Extension CheckValue="{tk:Extension/@CheckValue}"/>
      </xsl:if>
    </tk:Field>
  </xsl:template>
</xsl:stylesheet>
