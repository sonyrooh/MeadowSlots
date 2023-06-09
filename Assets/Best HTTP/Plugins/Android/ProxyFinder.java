package com.besthttp.proxy;

import java.net.InetSocketAddress;
import java.net.Proxy;
import java.net.ProxySelector;
import java.net.URI;
import java.util.Iterator;
import java.util.List;

public class ProxyFinder {

    public static String FindFor(String uriStr) {
        try {
			System.setProperty("java.net.useSystemProxies","true");

            List<Proxy> proxies = ProxySelector.getDefault().select(new URI(uriStr));

            for (Iterator<Proxy> iter = proxies.iterator(); iter.hasNext(); ) {
                Proxy proxy = iter.next();

                System.out.println("ProxyFinder.FindFor - proxy type : " + proxy.type());

                InetSocketAddress addr = (InetSocketAddress)proxy.address();

                if(addr == null) {
                    System.out.println("ProxyFinder.FindFor - No Proxy");
                } else {
                    System.out.println("ProxyFinder.FindFor - proxy hostname : " + addr.getHostName());
                    System.out.println("ProxyFinder.FindFor - proxy port : " + addr.getPort());

                    return String.format("%s://%s:%s", proxy.type(), addr.getHostName(), addr.getPort());
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
		
		return null;
    }
}
