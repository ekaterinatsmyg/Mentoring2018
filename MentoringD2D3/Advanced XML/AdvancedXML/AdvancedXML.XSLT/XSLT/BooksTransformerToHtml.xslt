<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
                xmlns:books="http://library.by/catalog"
                xmlns:user="urn:my-scripts"
                xmlns:fn ="http://www.w3.org/2005/xpath-functions">

  <msxsl:script implements-prefix='user' language='CSharp'>
    <![CDATA[
    public string curDate() {
      return System.DateTime.UtcNow.ToString();
    }]]>
  </msxsl:script>

  <!-- <xsl:variable name="genres" select="fn:distinct-values(/books:catalog/books:book/books:genre)" /> -->
  <xsl:variable name="genres">
    <genre>Computer</genre>
    <genre>Fantasy</genre>
    <genre>Romance</genre>
    <genre>Horror</genre>
    <genre>Science Fiction</genre>
  </xsl:variable>


  <xsl:output method="html" indent="yes"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>


  <xsl:template match="/books:catalog">
    <xsl:variable name="books" select="books:book"/>
    <html>
      <body>
        <h1>
          Current funds by genre
          <xsl:copy-of select = "user:curDate()" />
        </h1>

        <xsl:for-each select="msxsl:node-set($genres)/genre">
          <xsl:variable name="genre" select="."/>
          <table border="1" width="100%" style="margin-bottom:15px">
            <caption>
              <h4>
                <xsl:value-of select="$genre"/>
              </h4>
            </caption>

            <tr>
              <th>Author</th>
              <th>Title</th>
              <th>Publish Date</th>
              <th>Registration Date</th>
            </tr>

            <xsl:variable name="books-for-genre" select="$books[books:genre = $genre]"/>
            <xsl:for-each select="$books-for-genre">
              <tr>
                <td>
                  <xsl:value-of select="books:author"/>
                </td>
                <td>
                  <xsl:value-of select="books:title"/>
                </td>
                <td>
                  <xsl:value-of select="books:publish_date"/>
                </td>
                <td>
                  <xsl:value-of select="books:registration_date"/>
                </td>
              </tr>
            </xsl:for-each>

            <tr>
              <td colspan="4">
                <xsl:value-of select="$genre"/>
                Total:
                <xsl:value-of select="count($books-for-genre)"/>
              </td>
            </tr>
          </table>
        </xsl:for-each>

        <div>
          <h4>
            Total Books: <xsl:value-of select="count($books)"/>
          </h4>
        </div>
      </body>
    </html>


  </xsl:template>


</xsl:stylesheet>
