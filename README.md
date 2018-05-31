# SharpSettings
This is an extensions of SharpSettings.MongoDB. This provider adds the ability to hook into the ASP.NET Core settings provider to pull settings from a MongoDB data store.

| dev | master |
| --- | ------ |
| [![CircleCI](https://circleci.com/gh/thegreatco/SharpSettings.MongoDB.AspNet/tree/dev.svg?style=svg)](https://circleci.com/gh/thegreatco/SharpSettings.MongoDB.AspNet/tree/dev) | [![CircleCI](https://circleci.com/gh/thegreatco/SharpSettings.MongoDB.AspNet/tree/master.svg?style=svg)](https://circleci.com/gh/thegreatco/SharpSettings.MongoDB.AspNet/tree/master) |

See [SharpSettings](https://github.com/thegreatco/SharpSettings) for more general usage.
# Usage

WIP

### Logger
To be as flexible as possible and not requiring a particular logging framework, a shim must be implemented that implements the `ISharpSettingsLogger` interface. It follows similar patterns to `Serilog.ILogger` but is easily adapted to `Microsoft.Extensions.Logging` as well.