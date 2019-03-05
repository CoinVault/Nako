FROM destream-build

RUN  mkdir /root/.destreamnode \ 
	&& mkdir /root/.destreamnode/destream \
	&& mkdir /root/.destreamnode/destream/DeStreamMain

COPY destream.conf /root/.destreamnode/destream/DeStreamMain

VOLUME /root/.destreamnode/destream/DeStreamMain

WORKDIR /usr/local/bin/destream

RUN chmod +x DeStream.DeStreamD

EXPOSE 5000

ENTRYPOINT ["./DeStream.DeStreamD"]
