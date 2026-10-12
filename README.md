# unite-genome-liftover

Shared architecture, development workflows, and agent instructions:
[UNITE project documentation](https://github.com/dkfz-unite/.github/blob/main/README.md).

Draft service for converting supported omics data between genome references
GRCh37 and GRCh38 in both directions. It is not currently used in the platform.

The owner plans separate installations for each genome build. Users may want
data available for GRCh37 on a GRCh38 instance, or the reverse; this service is
intended to convert what can be converted. Coverage of individual data types
and the integration workflow are not yet established.

The draft does not change the current installation-specific genome-build
configuration or provide an already available automatic conversion pipeline.
