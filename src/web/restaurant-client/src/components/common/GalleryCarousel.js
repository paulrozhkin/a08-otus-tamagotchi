import React from 'react';
import {Image} from 'react-bootstrap';
import {imageService} from "../../services/image-service";

function GalleryCarousel(props) {

    const {images} = props

    let content
    if (images.length > 0) {
        content = images.map(imageId => {
            return (<div key={imageId} className="my-4">
                <Image fluid src={imageService.getUrlById(imageId)}/>
            </div>)
        })
    }

    return (
        <>
            {content}
        </>
    );
}

export default GalleryCarousel;
