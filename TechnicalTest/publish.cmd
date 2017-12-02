dotnet publish --configuration Release -o ./publish
docker build -t aspnetapp .
docker run -it --rm --name aspnetcore_sample aspnetapp