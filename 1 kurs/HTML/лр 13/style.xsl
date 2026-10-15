<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
<html>
<head>
<style>
table {
   border:  1px solid rgb(57, 13, 13);
   border-collapse: collapse;
}
th {
   border: 1px solid rgb(57, 13, 13);
   padding: 5px;
   text-align: center;
   background-color: rgb(217, 161, 180);
}
td {
   border: 1px solid rgb(57, 13, 13);
   padding: 5px;
   text-align: center;
   background-color: rgba(186, 168, 174, 0.557);
}
</style>
</head>
<body>
<h2>My CD Collection</h2>
<table border="1">
<xsl:for-each select="holidayList/head">
<xsl:sort select="hname"/>
<xsl:sort select="hdate"/>
   <tr>
      <th><xsl:value-of select="hname"/></th>
      <th><xsl:value-of select="hdate"/></th>
   </tr>
</xsl:for-each>
<xsl:for-each select="holidayList/row">
<xsl:sort select="name"/>
<xsl:sort select="date"/>
   <tr>
      <td><xsl:value-of select="name"/></td>
      <td><xsl:value-of select="date"/></td>
   </tr>
</xsl:for-each>
</table>
</body>
</html>
</xsl:template>

</xsl:stylesheet> 