# Change Log

## [2.0.0]

### Breaking Changes

- Changed interfaces to use `IEnumerable<string>` for tags instead of `string[]`. This elimiates the overhead of copying
  arrays in consuming code when building out tags.

## [1.0.0]

Inital release.

Basic support for:

- DataDog
- Amazon CloudWatch
