#!/bin/bash

dir="chains"

if [ ! -d "$dir" ]
then
  mkdir $dir
fi

if [ ! -f "$dir/hg19ToHg38.over.chain.gz" ]
then
  wget -O "$dir/hg19ToHg38.over.chain.gz" http://hgdownload.soe.ucsc.edu/goldenPath/hg19/liftOver/hg19ToHg38.over.chain.gz
fi

if [ ! -f "$dir/hg38ToHg19.over.chain.gz" ]
then
  wget -O "$dir/hg38ToHg19.over.chain.gz" http://hgdownload.soe.ucsc.edu/goldenPath/hg38/liftOver/hg38ToHg19.over.chain.gz
fi
