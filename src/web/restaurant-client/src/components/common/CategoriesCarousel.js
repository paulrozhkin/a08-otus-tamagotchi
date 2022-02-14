import React, {useEffect, useState} from 'react';
import OwlCarousel from 'react-owl-carousel3';
import ProductBox from '../home/ProductBox';
import {dishesService} from "../../services/dishes-service";
import {imageService} from "../../services/image-service";

function CategoriesCarousel() {

    const [popularDishes, setPopularDishes] = useState([])

    useEffect(() => {
        dishesService.getPopularDishes()
            .then((dishes) => {
                setPopularDishes(dishes)
            })
    }, [])

    let content

    if (popularDishes.length > 0) {
        content = popularDishes.map(dish => {
            return <div className="item" key={dish.id}>
                <ProductBox
                    boxClass='osahan-category-item'
                    title={dish.name}
                    image={imageService.getUrlById(dish.photos[0])}
                    imageClass='img-fluid'
                    imageAlt='carousel'
                    linkUrl='#'
                />
            </div>
        })
    }

    return (
        <>
            {!!content &&
                <OwlCarousel nav loop {...options} className="owl-carousel-category owl-theme">
                    {content}
                </OwlCarousel>
            }
        </>
    );
}

const options = {
    responsive: {
        0: {
            items: 3,
        },
        600: {
            items: 4,
        },
        1000: {
            items: 6,
        },
        1200: {
            items: 8,
        },
    },
    loop: true,
    lazyLoad: true,
    autoplay: true,
    dots: false,
    autoplaySpeed: 1000,
    autoplayTimeout: 2000,
    autoplayHoverPause: true,
    nav: true,
    navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"]
}

export default CategoriesCarousel;
