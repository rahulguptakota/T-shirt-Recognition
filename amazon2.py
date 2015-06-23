import urllib
import mechanize
from bs4 import BeautifulSoup


for j in range(291, 292):
	url = "http://www.amazon.in/s/ref=lp_1968123031_pg_2?rh=n%3A1571271031%2Cn%3A%211571272031%2Cn%3A1968024031%2Cn%3A1968120031%2Cn%3A1968123031&page="+str(j)+"&ie=UTF8&qid=1432921997"

	br = mechanize.Browser()
	br.set_handle_robots(False)
	br.addheaders = [('User-agent', 'Firefox')]

	html = br.open(url).read()


	soup = BeautifulSoup(html)

	for i in range(j*48-48,j*48):
		for link in soup.findAll('li', {"id":"result_"+str(i)}):

			# print link.contents[0].findAll('img')[0]['src']
			# print link.contents[0].findAll('h2')[0].text
			# print link.contents[0].findAll('span',{"class":"a-size-base a-color-price s-price a-text-bold"})[0].text.encode('utf-8')
			try:
				product_url= link.contents[0].findAll('a')[0]['href']
				name=link.contents[0].findAll('h2')[0].text
				image_url = link.contents[0].findAll('img')[0]['src']
				price = link.contents[0].findAll('span',{"class":"a-size-base a-color-price s-price a-text-bold"})[0].text.encode('utf-8')
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
			

