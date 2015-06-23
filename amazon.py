import urllib
import mechanize
from bs4 import BeautifulSoup

url = "http://www.amazon.in/b/ref=m_casual_tees?_encoding=UTF8&ie=UTF8&node=1968123031&pf_rd_m=A1VBAL9TL5WCBF&pf_rd_s=merchandised-search-5&pf_rd_r=0KNC993MJQMWVR5PWDMP&pf_rd_t=101&pf_rd_p=570851847&pf_rd_i=5518664031"

br = mechanize.Browser()
br.set_handle_robots(False)
br.addheaders = [('User-agent', 'Firefox')]

html = br.open(url).read()


soup = BeautifulSoup(html)

for i in range(1,49):
	for link in soup.findAll('div', {"id":"result_"+str(i)}):
		try:
			product_url= link.contents[2].find('a')['href']
			name=link.contents[4].findAll('a')[0].text
			image_url = link.contents[2].findAll('img')[0]['src']
			price = link.contents[5].findAll('span', {"class":"bld lrg red"})[0].text.encode('utf-8')
			location = "F:\\tshirt recognition\\images\\"+name+".jpg"
			urllib.urlretrieve(str(image_url), location )
			print name,"\"", location,"\"", price,"\"",product_url
		except:
			pass
		# print link.contents[4].findAll('a')[0].text,"\"",
		# print link.contents[2].findAll('img')[0]['src'],"\"",
		# print link.contents[5].findAll('span', {"class":"bld lrg red"})[0].text.encode('utf-8')
		# name of item
		# print link.contents[4].findAll('a')[0].text


		# our image url is here
		# print link.contents[2].findAll('img')[0]['src']


		# our price is here
		# print link.contents[5].findAll('span', {"class":"bld lrg red"})[0].text.encode('utf-8')
		

