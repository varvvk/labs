

--6 тип пространственных данных
select column_name, data_type 
from INFORMATION_SCHEMA.COLUMNS
where TABLE_SCHEMA = 'dbo' and COLUMN_NAME = 'geom';
go

--7 srid
select top (1) geom.STSrid as srid_example
from dbo.[countries]
where geom is not null;
go

--8 атрибутивные столбцы
select column_name, data_type 
from INFORMATION_SCHEMA.COLUMNS
where TABLE_SCHEMA = 'dbo'
and TABLE_NAME = 'countries'
and DATA_TYPE not in ('geometry','geography');
go

--9 пространственные в wkt
select geom.STAsText() AS wkt
from dbo.[countries]
where geom is not null;
go

--10.1 пересечение
select  
a.name as Country1,
b.name as Country2,
a.geom.STIntersection(b.geom).ToString() AS IntersectionGeometry
from dbo.countries a, dbo.countries b
where a.name = 'Georgia' and b.name = 'Turkey';

--10.2 объединение
select
a.name as Country1,
b.name as Country2,
a.geom.STUnion(b.geom).ToString() as UnionGeometry
from dbo.countries a, dbo.countries b
where a.name = 'Belarus' and b.name = 'Poland';

--10.3 вложенность
select 
a.name as Country1,
b.name as Country2,
a.geom.STIntersects(b.geom) as Intersects
from dbo.countries a, dbo.countries b
where a.name = 'France' and b.name = 'Germany';

--10.4 упрощение - упрощение границы страны, уменьшение вершин
 select top 1
name as CountryName,
geom.STAsText() as OriginalGeometry,
geom.Reduce(0.1).STAsText() as SimplifiedGeometry
from dbo.countries;

--10.5 координаты вершин
select top 15
name as CountryName,
geom.STNumPoints() as NumberOfVertices
from dbo.countries
where geom is not null;

--10.6 размерность, 0=точка, 1=линия, 2=полигон
select top 15
name as CountryName,
geom.STDimension() as Dimension
from dbo.countries
where geom is not null;

--10.7 площадь и длина границы для каждой страны
select top 15
name as CountryName,
geom.STArea() as Area,
geom.STLength() as BoundaryLength
from dbo.countries
where geom is not null
order by geom.STArea() desc;

--10.8 расстояние между странами
select 
a.name as Country1,
b.name as Country2,
a.geom.STDistance(b.geom) as Distance
from dbo.countries a, dbo.countries b
where a.name = 'Russia' and b.name = 'Germany';

--11 создание точки
declare @point geometry = geometry::STPointFromText('POINT(13.4049 52.5200)', 4326);

select
'Берлин' as ObjectName,
@point.STAsText() as WKT_Geometry;

--линия
declare @line geometry = geometry::STLineFromText('LINESTRING(2.3522 48.8566, 4.3517 50.8503, 4.8952 52.3702)', 4326);

select
'Париж - Брюссель - Амстердам' as ObjectName,
@line.STAsText() as WKT_Geometry;

--полигон
declare @polygon geometry = geometry::STPolyFromText('POLYGON((-100 38, -95 38, -95 42, -100 42, -100 38))', 4326);

select
'США' as ObjectName,
@polygon.STAsText() as WKT_Geometry;

--12
--какая страна содержит точку
declare @point geometry = geometry::STPointFromText('POINT(15.4049 52.5200)', 4326);

select
name as CountryName,
geom.STContains(@point) as ContainsPoint
from dbo.countries
where geom.STContains(@point) = 1;

--страны которые пересечены линией
declare @line geometry = geometry::STLineFromText('LINESTRING(2.3522 48.8566, 4.3517 50.8503, 4.8952 52.3702)', 4326);

select 
name as CountryName,
geom.STIntersects(@line) as IntersectsLine
from dbo.countries
where geom.STIntersects(@line) = 1;

--кто пересекается полигоном
declare @polygon geometry = geometry::STPolyFromText('POLYGON((-100 38, -95 38, -95 42, -100 42, -100 38))', 4326);

select
name as CountryName,
geom.STIntersects(@polygon) as IntersectsPolygon
from dbo.countries
where geom.STIntersects(@polygon) = 1;