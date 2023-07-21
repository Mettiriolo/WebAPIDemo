### 创建Dockfile
```
cd MyMicroservice //去解决方案目录下
fsutil file createnew Dockerfile 0 //创建Dockfile文件
start Dockerfile // 在编辑器中打开
```
替换为以下内容
```
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY MyMicroservice.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MyMicroservice.dll"]
```
### 可选: 添加 .dockerignore 文件
A .dockerignore 文件减少了用作 `docker 生成` 一部分的文件集。更少的文件可使生成速度更快。
```
fsutil file createnew .dockerignore 0
```
替换为以下内容
```
Dockerfile
[b|B]in
[O|o]bj
```

### 创建Docker镜像（Image）
```
docker build -t mymicroservice .
```
- -t mymicroservice ：将镜像名称标记为mymicroservice（tag the image as mymicroservice）
- . :当前目录（Dockerfile文件所在目录）

查看镜像列表（list of all images）
```
docker images
```
### 运行Docker镜像（Run Docker iMage）
将应用运行在容器中
```
docker run -it --rm -p 3000:80 --name mymicroservicecontainer mymicroservice
```
- -p 3000:80：对主机（宿主）端口：容器端口
- mymicroservicecontainer：容器名称
- mymicroservice：镜像名称
