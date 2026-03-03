FROM continuumio/miniconda3

RUN conda install -y -c conda-forge -c bioconda ucsc-liftover

WORKDIR /data

ENV PATH=/opt/conda/bin:$PATH

CMD ["liftover"]
