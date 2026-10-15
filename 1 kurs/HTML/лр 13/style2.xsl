<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
<html>
<head>
<style>
table {
   border:  1px solid black;
   border-collapse: collapse;
}
th {
   border: 1px solid black;
   padding: 5px;
   text-align: center;
}
td {
   border: 1px solid black;
   padding: 5px;
   text-align: center;
}
.red {
    background-color:red;
}
.green {
    background-color:green;
}
</style>
</head>
<body>
    <table>
<tr>
            <th>Имя студента</th>
            <th>Оценка</th>
          </tr>
          <xsl:for-each select="students/student">
            <tr>
              <td><xsl:value-of select="name"/></td>
              <xsl:variable name="mark" select="mark"/>
              <td>
               <xsl:attribute name="class">
                <xsl:choose>
                  <xsl:when test="$mark &lt; 4">red</xsl:when>
                  <xsl:when test="$mark &gt; 8">green</xsl:when>
                </xsl:choose>
                </xsl:attribute>
                <xsl:value-of select="mark"/>
              </td>
            </tr>
          </xsl:for-each>
</table>
</body>
</html>
</xsl:template>

</xsl:stylesheet>