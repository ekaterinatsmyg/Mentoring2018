<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
                xmlns:books="http://library.by/catalog"
                xmlns:atom="http://www.w3.org/2005/Atom"
                xmlns:user="urn:my-scripts">

  <msxsl:script implements-prefix='user' language='CSharp'>
    <![CDATA[
    public string curDate() {
      return System.DateTime.UtcNow.ToString();
    }]]>
  </msxsl:script>

  <xsl:output method="xml" indent="yes" cdata-section-elements="atom:content"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="books:catalog">
    <xsl:element name="feed">
      <xsl:element name="title">Library</xsl:element>
      <xsl:element name="updated">
        <xsl:copy-of select = "user:curDate()" />
      </xsl:element>
      <xsl:element name="id">http://library.by/catalog</xsl:element>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="books:book">

    <xsl:element name ="entry">
      
      <xsl:element name ="id">
        <xsl:value-of select="@id"/>
      </xsl:element>
      
      <xsl:element name="title">
        <xsl:value-of select="books:title"/>
      </xsl:element>

      <xsl:element name="author">
        <xsl:element name="name">
          <xsl:value-of select="books:author"/>
        </xsl:element>
      </xsl:element>

      <xsl:element name="published">
        <xsl:value-of select="books:registration_date"/>
      </xsl:element>

      <xsl:variable name="isbn" select="books:isbn"/>
      <xsl:variable name="genre" select="books:genre"/>

      <xsl:element name="content">
        <xsl:value-of select="books:description"/>
        <xsl:if test="$isbn != '' and $genre = 'Computer'">
          <xsl:text>&#13;</xsl:text>
          <xsl:value-of select="concat('http://my.safaribooksonline.com/', $isbn , '/')" />
        </xsl:if>
        <xsl:text>&#13;</xsl:text>
        <xsl:value-of select="$genre"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  
</xsl:stylesheet>
