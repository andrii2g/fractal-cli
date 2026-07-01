# Algorithm Notes

Fractal CLI renders Mandelbrot and Julia images with the escape-time algorithm over the quadratic map:

```text
z(n + 1) = z(n)^2 + c
```

For each output pixel, the renderer maps the pixel to a point in the complex plane, iterates the formula, and colors the pixel based on whether and when the orbit escapes.

## Mandelbrot Set

For Mandelbrot rendering, each pixel supplies the constant `c`, and the iteration starts at `z = 0`.

```text
z0 = 0
c = pixelX + pixelY * i
z(n + 1) = z(n)^2 + c
```

The point is treated as outside the set when:

```text
real(z)^2 + imag(z)^2 > 4
```

If the point does not escape before `--max-iterations`, it is treated as interior and rendered black.

References:

- [Mandelbrot set](https://en.wikipedia.org/wiki/Mandelbrot_set)
- [Plotting algorithms for the Mandelbrot set](https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set)

## Julia Set

For Julia rendering, each pixel supplies the starting value `z`, and the CLI options `--julia-cx` and `--julia-cy` supply the fixed constant `c`.

```text
z0 = pixelX + pixelY * i
c = juliaCx + juliaCy * i
z(n + 1) = z(n)^2 + c
```

The same escape radius is used:

```text
real(z)^2 + imag(z)^2 > 4
```

References:

- [Julia set](https://en.wikipedia.org/wiki/Julia_set)
- [Filled Julia set](https://en.wikipedia.org/wiki/Julia_set#Filled_Julia_set)

## Viewport Mapping

The CLI uses `--center-x`, `--center-y`, and `--scale` to define the complex-plane viewport. `--scale` is the horizontal span. The vertical span is derived from the image aspect ratio:

```text
xSpan = scale
ySpan = scale * height / width
```

This keeps the image from stretching the fractal when width and height differ.

For a pixel coordinate `(x, y)`:

```text
complexX = xMin + x * (xMax - xMin) / (width - 1)
complexY = yMax - y * (yMax - yMin) / (height - 1)
```

The Y axis is inverted so the top row of the image maps to the maximum imaginary coordinate.

## Coloring

The first version uses normalized iteration-count coloring:

```text
t = escapedIterations / maxIterations
```

Points that do not escape are black. Escaped points are mapped through one of the built-in deterministic palettes:

- `classic`
- `fire`
- `ice`
- `gray`

The renderer intentionally does not use smooth coloring yet. Smooth escape-time coloring can reduce banding, but the first version keeps the algorithm simple and predictable.

Reference:

- [Escape-time algorithm](https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set#Escape_time_algorithm)

## Implementation Notes

The implementation avoids per-pixel allocations:

- complex values are stored as separate `double` variables
- rendering writes to an `Rgba32[]` pixel buffer
- rows are rendered in parallel with `Parallel.For`
- ImageSharp is used only to encode the final PNG

The code uses double precision and a fixed bailout radius squared of `4.0`, which is the standard threshold for quadratic Mandelbrot and Julia escape-time rendering.
