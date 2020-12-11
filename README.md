# TinySeoSharp

Tiny project that explores translating a route template to
a C# model and back in a way that is SEO friendly.

A key feature is that fuzzy matching is used
to build or match the route template. This allows the
route to be more flexible to changes for SEO without
the need to redefine the template, enum, or add additional
processing logic to get back to the canonical page.

Another feature is that a route can be built using the
template and a supplied query object. The resulting route
will be SEO optimized and enforce consiste rules.
