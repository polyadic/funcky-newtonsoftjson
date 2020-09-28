# Funcky.NewtonsoftJson

[![Build](https://github.com/polyadic/funcky-newtonsoftjson/workflows/Build/badge.svg)](https://github.com/polyadic/funcky-newtonsoftjson/actions?query=workflow%3ABuild)
[![Licence: MIT](https://img.shields.io/badge/licence-MIT-green)](https://raw.githubusercontent.com/polyadic/funcky-newtonsoftjson/master/LICENSE-MIT)
[![Licence: Apache](https://img.shields.io/badge/licence-Apache-green)](https://raw.githubusercontent.com/polyadic/funcky-newtonsoftjson/master/LICENSE-Apache)

## Usage
```csharp
using Funcky.Monads;
using Funcky.NewtonsoftJson;
using Newtonsoft.Json;

var settings = new JsonSerializerSettings().AddOptionConverter();
var json = JsonConvert.SerializeObject(Option.Some("hello world"), settings);
```
