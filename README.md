# CefGlue
.NET binding for The Chromium Embedded Framework (CEF). 

CefGlue lets you embed Chromium in .NET apps. It is a .NET wrapper control around the Chromium Embedded Framework ([CEF](https://bitbucket.org/chromiumembedded/cef/src/master/)). 
This fork targets **.NET 10** and **Avalonia 12**, and aims to support **Avalonia on Windows and Linux only** (macOS and WPF are not targeted).

It was initially branched from `selway/CefGlue` (`feature-avaloniaui12` at commit `e204172`, [OutSystems/CefGlue#249](https://github.com/OutSystems/CefGlue/pull/249)), with subsequent upstream `OutSystems/CefGlue` fixes, PR #249 follow-ups, and `.NET 10` / Avalonia 12 updates applied on top.

Here's a table for supported architectures, frameworks and operating systems in this fork:

| OS      | x64 | ARM64 | WPF | Avalonia |
|---------|-----|-------|-----|----------|
| Windows | ✔️  | ✔️    | ❌  | ✔️      |
| macOS   | ❌  | ❌    | ❌  | ❌      |
| Linux   | ✔️  | 🔘    | ❌  | ✔️      |

✔️ Supported
❌ Not supported / not targeted in this fork
🔘 Works with issues.

See [LINUX.md](./LINUX.md) for more information about issues and tested distribution list. 

Currently only x64 and ARM64 architectures are supported.

## Releases
This repository is a fork branch and does not publish prebuilt binaries or NuGet packages. Please build the projects directly from source.

## Documentation 
See the [Avalonia sample](CefGlue.Demo.Avalonia) project for an example web browser built with CefGlue.

To clean-build and launch the Avalonia demo for your current OS/architecture:
```bash
./run_demo.sh
# or for Release configuration:
CONFIGURATION=Release ./run_demo.sh
```
