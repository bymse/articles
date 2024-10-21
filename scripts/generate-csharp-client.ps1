dotnet build ../Bymse.Articles/Bymse.Articles.Apis -c Debug
dotnet tool restore
dotnet swagger tofile `
--yaml `
--output ../Bymse.Articles/Bymse.Articles.Apis/docs/openapi.yaml `
../Bymse.Articles/Bymse.Articles.Apis/bin/Debug/net8.0/Bymse.Articles.Apis.dll `
public-api 